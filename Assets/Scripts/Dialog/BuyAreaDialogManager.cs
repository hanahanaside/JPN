using UnityEngine;
using System.Collections;

public class BuyAreaDialogManager : MonoSingleton<BuyAreaDialogManager> {

	public UILabel areaNameLabel;
	public UILabel costLabel;

	private Area mArea;

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		gameObject.transform.localScale = new Vector3 (1,1,1);
		gameObject.SetActive (false);
	}

	public void Show (Area area) {
		mArea = area;
		areaNameLabel.text = area.AreaName;
		costLabel.text = "" + area.AreaOpen;
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}

	public void CancelClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void BuyClicked () {
		//所持金の確認
		if(PlayerDataKeeper.instance.CoinCount < mArea.AreaOpen){
			return;
		}
		//各ステージのクリア回数を取得
		int[] clearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		//購入したステージのクリア回数を-1から0にする
		clearedPuzzleCountArray [mArea.AreaId -1] = 0;
		//最後のステージでなければ次のステージを未購入の状態にする
		if(mArea.AreaId != 8){
			clearedPuzzleCountArray [mArea.AreaId] = -1;
		}
		PrefsManager.instance.ClearedPuzzleCountArray = clearedPuzzleCountArray;
		PlayerDataKeeper.instance.DecreaseCoinCount (mArea.AreaOpen);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void UseTicketClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnFenceClicked () {
		if (gameObject.activeSelf) {
			Dismiss ();
		}
	}

	private void Dismiss(){
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
	}
}
