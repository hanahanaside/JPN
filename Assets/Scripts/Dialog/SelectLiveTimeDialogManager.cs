using UnityEngine;
using System.Collections;

public class SelectLiveTimeDialogManager : MonoBehaviour {

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		gameObject.transform.localScale = new Vector3 (1,1,1);
		gameObject.SetActive (false);
	}
		
	public void On1MinuteClicked(){
		FenceManager.instance.HideFence ();
		LiveManager.instance.StartLive (10.0f);
		PlayerDataKeeper.instance.DecreaseTicketCount (1);
		gameObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void On1HourClicked(){
		FenceManager.instance.HideFence ();
		LiveManager.instance.StartLive (30.0f);
		PlayerDataKeeper.instance.DecreaseTicketCount (2);
		gameObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void On8HoursClicked(){
		FenceManager.instance.HideFence ();
		LiveManager.instance.StartLive (60.0f);
		PlayerDataKeeper.instance.DecreaseTicketCount (3);
		gameObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnCloseClicked(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}
}
