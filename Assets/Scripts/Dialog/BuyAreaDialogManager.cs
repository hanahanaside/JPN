using UnityEngine;
using System.Collections;

public class BuyAreaDialogManager : MonoSingleton<BuyAreaDialogManager> {

	public UILabel areaNameLabel;
	public UILabel costLabel;
	public UILabel ticketCostLabel;
	private GameObject mDialogObject;

	private Area mArea;

	public override void OnInitialize(){
		mDialogObject = transform.Find ("Dialog").gameObject;
	}

	void CompleteDismissEvent () {
		FenceManager.instance.HideFence ();
		gameObject.transform.localScale = new Vector3 (1, 1, 1);
		mDialogObject.SetActive (false);
	}

	public void Show (Area area) {
		mDialogObject.SetActive (true);
		mArea = area;
		areaNameLabel.text = area.AreaName;
		costLabel.text = "" + area.AreaOpen;
		ticketCostLabel.text = "×" + (area.AreaOpen / 4000);
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}

	public void CancelClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void BuyClicked () {
		//所持金の確認
		if (PlayerDataKeeper.instance.CoinCount < mArea.AreaOpen) {
			return;
		}
		BuyArea ();
		PlayerDataKeeper.instance.DecreaseCoinCount (mArea.AreaOpen);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void UseTicketClicked () {
		BuyArea ();
		PlayerDataKeeper.instance.DecreaseTicketCount (mArea.AreaOpen / 100);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	private void BuyArea(){
		//各ステージのクリア回数を取得
		int[] clearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		//購入したステージのクリア回数を-1から0にする
		clearedPuzzleCountArray [mArea.AreaId - 1] = 0;
		//最後のステージでなければ次のステージを未購入の状態にする
		if (mArea.AreaId != 8) {
			clearedPuzzleCountArray [mArea.AreaId] = -1;
		}
		PrefsManager.instance.ClearedPuzzleCountArray = clearedPuzzleCountArray;
	}

	private void Dismiss () {
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
	}
}
