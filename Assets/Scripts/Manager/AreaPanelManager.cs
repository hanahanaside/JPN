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

	public event Action<int,int> OnAreaClickedEvent;

	public UIScrollView areaScrollView;
	public UIGrid grid;
	public GameObject dialogObject;

	private Entity_Area mEntityArea;
	private int[] mClearedPuzzleCountArray;

	void Awake () {
		//マスターデータを取得
		mEntityArea = Resources.Load ("Data/Area") as Entity_Area; //=> Resourcesからデータファイルの読み込み
		//各ステージのクリア回数を取得
		mClearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
	}

	void MoveOutEventCompleted () {
		dialogObject.transform.localPosition = new Vector3 (0, 0, 0);
		dialogObject.SetActive (false);
	}

	public void ShowAreaPanel () {
		FenceManager.instance.ShowFence ();
		dialogObject.SetActive (true);
		areaScrollView.ResetPosition ();
		List<Transform> childList = grid.GetChildList ();
		for (int i = 0; i < childList.Count; i++) {
			int clearedCount = mClearedPuzzleCountArray [i];
			UILabel label = childList [i].Find ("Label").GetComponent<UILabel> ();
			switch (clearedCount) {
			//未購入の場合
			case (int)AreaState.NotYetPurchased:
				label.text = mEntityArea.param [i].minimum_amount + "人でオープン";
				break;
			//ロックの場合
			case (int)AreaState.Lock:
				label.text = "ロック";
				break;
			//デフォルト
			default:
				int cost = mEntityArea.param [i].cost_start + (mEntityArea.param [i].cost_add * mClearedPuzzleCountArray [i]);
				StringBuilder sb = new StringBuilder ();
				sb.Append (mEntityArea.param [i].area_name + "\n");
				sb.Append (cost + "コイン");
				label.text = sb.ToString ();
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
			break;
		case (int)AreaState.Lock:
			break;
		default:
			int cost = mEntityArea.param [(int)areaName].cost_start + (mEntityArea.param [(int)areaName].cost_add * clearCount);
			FenceManager.instance.HideFence ();
			ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject, gameObject);
			OnAreaClickedEvent ((int)areaName,cost);
			break;
		}
	}

	public void OnCloseButtonClicked () {
		FenceManager.instance.HideFence ();
		ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject, gameObject);
	}
}
