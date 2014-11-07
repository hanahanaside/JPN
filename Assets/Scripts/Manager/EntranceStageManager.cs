using UnityEngine;
using System.Collections;

public class EntranceStageManager : MonoSingleton<EntranceStageManager>{

	public void OnLiveButtonClicked(){
		LiveManager.instance.StartLive ();
	}

	public void StartLive(){

	}
}
