using UnityEngine;
using System.Collections;

public class SelectLiveTimeDialogManager : MonoBehaviour {

	public void On1MinuteClicked(){
		FenceManager.instance.HideFence ();
		LiveManager.instance.StartLive (10.0f);
		gameObject.SetActive (false);
	}

	public void On1HourClicked(){
		FenceManager.instance.HideFence ();
		LiveManager.instance.StartLive (30.0f);
		gameObject.SetActive (false);
	}

	public void On8HoursClicked(){
		FenceManager.instance.HideFence ();
		LiveManager.instance.StartLive (60.0f);
		gameObject.SetActive (false);
	}

	public void OnCloseClicked(){
		FenceManager.instance.HideFence ();
		gameObject.SetActive (false);
	}
}
