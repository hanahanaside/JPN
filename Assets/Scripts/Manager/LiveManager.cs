using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveManager : MonoSingleton<LiveManager> {

	public GameObject livePanelObject;

	private float mTime;
	private bool mLive;
	private GameObject mirrorBallSpriteObject;
	private UILabel mRemainingLiveTimeLabel;

	void Awake(){
		mirrorBallSpriteObject = livePanelObject.transform.Find ("MirroBallSprite").gameObject;
		mRemainingLiveTimeLabel = livePanelObject.transform.Find ("RemainingTimeLabel").GetComponent<UILabel>();
	}

	// Update is called once per frame
	void Update () {
		if (!mLive) {
			return;
		}	
		mTime -= Time.deltaTime;
		mRemainingLiveTimeLabel.text = "" + (int)mTime;
		if (mTime > 0) {
			return;
		}
		FinishLive ();
	}

	void OnCompleteMirrorBallFinishLiveEvent(){
		livePanelObject.SetActive (false);
		EntranceStageManager.instance.FinishLive ();
	}

	public void StartLive (float time) {
		mTime = time;
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.StartLive ();
		}
		livePanelObject.SetActive (true);
		mLive = true;
		iTweenEvent.GetEvent (mirrorBallSpriteObject,"LiveStartEvent").Play();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Live);
	}

	private void FinishLive(){
		mLive = false;
		iTweenEvent.GetEvent (mirrorBallSpriteObject,"LiveFinishEvent").Play();
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.FinishLive ();
		}
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
	}
}
