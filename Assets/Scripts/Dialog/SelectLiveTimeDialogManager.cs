using UnityEngine;
using System.Collections;

public class SelectLiveTimeDialogManager : MonoBehaviour {

	public void On1MinuteClicked(){
		LiveManager.instance.StartLive (10.0f);
		gameObject.SetActive (false);
	}

	public void On1HourClicked(){
		LiveManager.instance.StartLive (30.0f);
		gameObject.SetActive (false);
	}

	public void On8HoursClicked(){
		LiveManager.instance.StartLive (60.0f);
		gameObject.SetActive (false);
	}

	public void OnCloseClicked(){
		gameObject.SetActive (false);
	}
}
