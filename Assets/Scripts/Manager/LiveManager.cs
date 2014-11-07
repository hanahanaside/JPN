using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveManager : MonoSingleton<LiveManager> {

	public GameObject livePanelObject;

	private float mTime;
	private bool mLive;

	// Update is called once per frame
	void Update () {
		if (!mLive) {
			return;
		}	
		mTime -= Time.deltaTime;
		if (mTime > 0) {
			return;
		}
		FinishLive ();
	}

	public void StartLive () {
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.StartLive ();
		}
		mTime = 10.0f;
		livePanelObject.SetActive (true);
		mLive = true;
	}

	private void FinishLive(){
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.FinishLive ();
		}
		mLive = false;
		livePanelObject.SetActive (false);
	}
}
