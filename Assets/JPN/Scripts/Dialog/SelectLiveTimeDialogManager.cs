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
			LiveManager.instance.StartLive (60);
			PlayerDataKeeper.instance.DecreaseTicketCount (1);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void On5MinuteClicked(){ 
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		if(PlayerDataKeeper.instance.TicketCount >= 3){
			LiveManager.instance.StartLive (300);
			PlayerDataKeeper.instance.DecreaseTicketCount (3);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void On10MinuteClicked(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(PlayerDataKeeper.instance.TicketCount >= 5){
			LiveManager.instance.StartLive (600);
			PlayerDataKeeper.instance.DecreaseTicketCount (5);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void On30MinuteClicked(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(PlayerDataKeeper.instance.TicketCount >= 8){
			LiveManager.instance.StartLive (1800);
			PlayerDataKeeper.instance.DecreaseTicketCount (8);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void On60MinuteClicked(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(PlayerDataKeeper.instance.TicketCount >=10){
			LiveManager.instance.StartLive (3600);
			PlayerDataKeeper.instance.DecreaseTicketCount (10);
		}else {
			BuyTicketDialog.instance.Show ();
		}
	}

	public void On600MinuteClicked(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(PlayerDataKeeper.instance.TicketCount >= 50){
			LiveManager.instance.StartLive (36000);
			PlayerDataKeeper.instance.DecreaseTicketCount (50);
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
