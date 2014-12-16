using UnityEngine;
using System.Collections;

public class EntranceStageManager : MonoSingleton<EntranceStageManager>{

	public GameObject startLiveButton;

	public void OnLiveButtonClicked(){
		startLiveButton.SetActive (false);
		LiveManager.instance.StartLive ();
	}

	public void StartLive(){

	}

	public void FinishLive(){
		startLiveButton.SetActive (true);
	}
}
