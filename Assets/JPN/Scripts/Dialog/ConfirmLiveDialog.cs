using UnityEngine;
using System.Collections;

public class ConfirmLiveDialog : MonoSingleton<ConfirmLiveDialog> {

	private GameObject mDialogObject;

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public override void OnInitialize(){
		mDialogObject = transform.Find ("Dialog").gameObject;
	}

	public void Show(){
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (mDialogObject,"ShowEvent").Play();
	}

	public void Dissmiss(){
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
	}

	public void StartLiveButtonClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button); 
		if(PlayerDataKeeper.instance.TicketCount >=1){
			LiveManager.instance.StartLive (60);
			PlayerDataKeeper.instance.DecreaseTicketCount (1);
		}else {
			BuyTicketDialog.instance.Show ();
		}
		Dissmiss ();
	}

	public void CloseButtonClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button); 
		Dissmiss ();
	}
}
