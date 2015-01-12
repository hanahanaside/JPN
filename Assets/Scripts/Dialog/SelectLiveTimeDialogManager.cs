using UnityEngine;
using System.Collections;

public class SelectLiveTimeDialogManager : MonoSingleton<SelectLiveTimeDialogManager> {

	private GameObject mDialogObject;

	public override void OnInitialize(){
		mDialogObject = transform.Find ("Dialog").gameObject;
	}

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		gameObject.transform.localScale = new Vector3 (1,1,1);
		mDialogObject.SetActive (false);
	}

	public void Show(){
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}
		
	public void On1MinuteClicked(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(PlayerDataKeeper.instance.TicketCount >=1){
			LiveManager.instance.StartLive (10.0f);
			PlayerDataKeeper.instance.DecreaseTicketCount (1);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void On1HourClicked(){ 
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		if(PlayerDataKeeper.instance.TicketCount >= 2){
			LiveManager.instance.StartLive (30.0f);
			PlayerDataKeeper.instance.DecreaseTicketCount (2);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void On8HoursClicked(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(PlayerDataKeeper.instance.TicketCount >= 3){
			LiveManager.instance.StartLive (180.0f);
			PlayerDataKeeper.instance.DecreaseTicketCount (3);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void OnCloseClicked(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}
}
