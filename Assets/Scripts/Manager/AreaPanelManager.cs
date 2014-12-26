using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

public class AreaPanelManager : MonoSingleton<AreaPanelManager> {

	private enum AreaName {
		SugeKita,
		ChoiKita,
		ChoiMinami,
		Chubu,
		Kansai,
		Chugoku,
		Shikokyu,
		Kyushu}
	;

	private enum AreaState {
		NotYetPurchased = -1,
		Lock = -2
	}

	public event Action<int> OnAreaClickedEvent;

	public UIScrollView areaScrollView;
	public UIGrid grid;
	public GameObject dialogObject;

	private Entity_Area mEntityArea;
	private int[] mClearedPuzzleCountArray;

	void Awake () {
		//マスターデータを取得
		mEntityArea = Resources.Load ("Data/Area") as Entity_Area; //=> Resourcesからデータファイルの読み込み
	}

	void MoveOutEventCompleted () {
		dialogObject.transform.localPosition = new Vector3 (0, 0, 0);
		dialogObject.SetActive (false);
	}

	public void ShowAreaPanel () {
		//各ステージのクリア回数を取得
		mClearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		FenceManager.instance.ShowFence ();
		dialogObject.SetActive (true);
		areaScrollView.ResetPosition ();
		List<Transform> childList = grid.GetChildList ();
		for (int i = 0; i < childList.Count; i++) {
			int clearedCount = mClearedPuzzleCountArray [i];
			UILabel areaLabel = childList [i].Find ("AreaLabel").GetComponent<UILabel> ();
			UILabel descriptionLabel = childList [i].Find ("DescriptionLabel").GetComponent<UILabel> ();
			UILabel costLabel = childList [i].Find ("CostLabel").GetComponent<UILabel> ();
			GameObject coinObject = childList [i].Find ("Sprite").gameObject;
			areaLabel.text = mEntityArea.param [i].area_name;
			switch (clearedCount) {
			//未購入の場合
			case (int)AreaState.NotYetPurchased:
				coinObject.SetActive (false);
				descriptionLabel.text = "購入可能";
				costLabel.text = "";
				childList [i].gameObject.collider.enabled = true;
				break;
			//ロックの場合
			case (int)AreaState.Lock:
				coinObject.SetActive (false);
				descriptionLabel.text = "ロック";
				costLabel.text = "";
				childList [i].GetComponent<UIButton> ().state = UIButtonColor.State.Disabled;
				childList [i].gameObject.collider.enabled = false;
				break;
			//デフォルト
			default:
				int cost = AreaCostCaluculator.instance.CalcCost (i);
				costLabel.text = "" +cost;
				coinObject.SetActive (true);
				descriptionLabel.text = "";
				childList [i].gameObject.collider.enabled = true;
				break;
			}
		}
		ItweenEventPlayer.PlayMoveInDialogEvent (dialogObject);
	}

	public void OnSugeKitaClicked () {
		OnAreaButtonClicked (AreaName.SugeKita);
	}

	public void OnChoiKitaClicked () {
		OnAreaButtonClicked (AreaName.ChoiKita);
	}

	public void OnChoiMinamiClicked () {
		OnAreaButtonClicked (AreaName.ChoiMinami);
	}

	public void OnChubuClicked () {
		OnAreaButtonClicked (AreaName.Chubu);
	}

	public void OnKansaiClicked () {
		OnAreaButtonClicked (AreaName.Kansai);
	}

	public void OnChugokuClicked () {
		OnAreaButtonClicked (AreaName.Chugoku);
	}

	public void OnShikokyuClicked () {
		OnAreaButtonClicked (AreaName.Shikokyu);
	}

	public void OnKyushuClicked () {
		OnAreaButtonClicked (AreaName.Kyushu);
	}

	private void OnAreaButtonClicked(AreaName areaName){
		int clearCount = mClearedPuzzleCountArray[(int)areaName];
		switch(clearCount){
		case (int)AreaState.NotYetPurchased:
			ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject, gameObject);
			ShowBuyAreaDialog (areaName);
			break;
		case (int)AreaState.Lock:
			break;
		default:
			FenceManager.instance.HideFence ();
			ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject, gameObject);
			OnAreaClickedEvent ((int)areaName);
			break;
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	private void ShowBuyAreaDialog(AreaName areaName){
		Area area = new Area ();
		area.AreaId = mEntityArea.param [(int)areaName].area_id;
		area.AreaName = mEntityArea.param[(int)areaName].area_name;
		area.AreaOpen = mEntityArea.param [(int)areaName].area_open;
		BuyAreaDialogManager.instance.Show (area);
	}

	public void OnCloseButtonClicked () {
		FenceManager.instance.HideFence ();
		ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject, gameObject);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}
}
