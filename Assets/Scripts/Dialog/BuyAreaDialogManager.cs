using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyAreaDialogManager : MonoSingleton<BuyAreaDialogManager> {

	public UILabel areaNameLabel;
	public UILabel costLabel;
	public UILabel ticketCostLabel;
	public UILabel conditionLabel;
	public UILabel descriptionLabel;
	public GameObject buyButtonObject;
	private GameObject mDialogObject;
	private int mCostTicket;

	private Area mArea;

	public override void OnInitialize () {
		mDialogObject = transform.Find ("Dialog").gameObject;
	}

	void CompleteDismissEvent () {
		gameObject.transform.localScale = new Vector3 (1, 1, 1);
		mDialogObject.SetActive (false);
	}

	public void Show (Area area) {
		mDialogObject.SetActive (true);
		mArea = area;
		mCostTicket = area.AreaOpen / 2500;
		if(mCostTicket == 0){
			mCostTicket = 1;
		}
		areaNameLabel.text = area.AreaName;
		costLabel.text = "" + area.AreaOpen; 
		descriptionLabel.text = area.AreaName + "でスカウトするには、\n入場料を払う必要があります";
		ticketCostLabel.text = "×" + (mCostTicket); 
		int totalIdleCount = 0;
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		foreach (Stage stage in stageList) {
			totalIdleCount += stage.IdleCount;
		}
		if (totalIdleCount < area.MinimumAmount) {
			conditionLabel.text = "アイドルの数が" + (area.MinimumAmount - totalIdleCount) + "人不足しています";
			buyButtonObject.SetActive (false);
		} else {
			conditionLabel.text = "購入できます";
		}
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}

	public void CancelClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void BuyClicked () {
		//所持金の確認
		if (PlayerDataKeeper.instance.CoinCount < mArea.AreaOpen) {
			Dismiss ();
			FenceManager.instance.ShowFence ();
			OKDialog.instance.Show ("コインが不足しています");
			return;
		}
		BuyArea ();
		PlayerDataKeeper.instance.DecreaseCoinCount (mArea.AreaOpen);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void UseTicketClicked () {
		if (PlayerDataKeeper.instance.TicketCount < mCostTicket) {
			Dismiss ();
			FenceManager.instance.ShowFence ();
			OKDialog.instance.Show ("チケットが不足しています");
			return;
		}
		BuyArea ();
		PlayerDataKeeper.instance.DecreaseTicketCount (mCostTicket);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	private void BuyArea () {
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
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
	}
}
