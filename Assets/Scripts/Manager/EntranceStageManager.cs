using UnityEngine;
using System.Collections;

public class EntranceStageManager : MonoSingleton<EntranceStageManager>{

	public GameObject startLiveButton;

	public void OnLiveButtonClicked(){
		FenceManager.instance.ShowFence ();
		SelectLiveTimeDialogManager.instance.Show ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void StartLive(){
		startLiveButton.SetActive (false);
	}

	public void FinishLive(){
		startLiveButton.SetActive (true);
	}
}
