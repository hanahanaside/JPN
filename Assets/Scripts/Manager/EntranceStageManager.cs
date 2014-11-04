using UnityEngine;
using System.Collections;

public class EntranceStageManager : StageManager<EntranceStageManager>{

	public void OnLiveButtonClicked(){
		StageGridManager.instance.StartLive ();
	}

	public override void StartLive(){

	}
}
