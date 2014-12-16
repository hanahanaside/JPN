using UnityEngine;
using System.Collections;

public class EntranceStageManager : MonoSingleton<EntranceStageManager>{

	public GameObject startLiveButton;
	public GameObject selectLiveTimeDialog;

	public void OnLiveButtonClicked(){
		FenceManager.instance.ShowFence ();
		selectLiveTimeDialog.SetActive (true);
		iTweenEvent.GetEvent (selectLiveTimeDialog,"ShowEvent").Play();
	}

	public void StartLive(){
		startLiveButton.SetActive (false);
	}

	public void FinishLive(){
		startLiveButton.SetActive (true);
	}
}
