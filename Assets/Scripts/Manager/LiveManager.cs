using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveManager : MonoSingleton<LiveManager> {

	public GameObject[] curtainArray;
	public GameObject[] ballArray;
	public GameObject ballParent;
	public GameObject curtainHeadObject;
	public GameObject logoObject;

	private float mTime;
	private bool mLive;
	private GameObject mirrorBallSpriteObject;
	private GameObject spinTextureObject;
	private Vector3[] mStartCurtainPosition;
	private UILabel mRemainingLiveTimeLabel;
	public GameObject livePanelObject;

	void Awake () {
		mirrorBallSpriteObject = livePanelObject.transform.Find ("MirroBallSprite").gameObject;
		spinTextureObject = livePanelObject.transform.Find ("SpinTexture").gameObject;
		mRemainingLiveTimeLabel = livePanelObject.transform.Find ("RemainingTimeLabel").GetComponent<UILabel> ();
		mStartCurtainPosition = new Vector3[2];
		for (int i = 0; i < curtainArray.Length; i++) {
			mStartCurtainPosition [i] = curtainArray [i].transform.localPosition;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!mLive) {
			return;
		}	
		mTime -= Time.deltaTime;
		mRemainingLiveTimeLabel.text = TimeConverter.Convert (mTime);
		if (mTime > 0) {
			return;
		}
		FinishLive ();
	}

	void OnCompleteMirrorBallFinishLiveEvent () {
		livePanelObject.SetActive (false);
		for (int i = 0; i < curtainArray.Length; i++) {
			curtainArray [i].transform.localPosition = mStartCurtainPosition [i];
		}
		EntranceStageManager.instance.FinishLive ();
	}

	void OpenedCurtainEvent () {
		curtainHeadObject.SetActive (false);
	}

	public void StartLive (float time) {
		livePanelObject.SetActive (true);
		curtainHeadObject.SetActive (true);
		spinTextureObject.transform.localEulerAngles = new Vector3 (0, 0, 0);
		mTime = time + 17;
		CoinGenerator.instance.StopGenerating ();
		FenceManager.instance.ShowFence ();
		EntranceStageManager.instance.StartLive ();
		ballParent.transform.localPosition = new Vector3 (0, 0, 0);
		foreach (GameObject ballObject in ballArray) {
			ballObject.transform.localPosition = new Vector3 (0, 0, 0);
		}
		SoundManager.instance.StopBGM ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Cheer);
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.StartLive ();
		}
		LiveData liveData = new LiveData ();
		liveData.startDate = System.DateTime.Now.ToString ();
		liveData.time = mTime;
		PrefsManager.instance.WriteData<LiveData> (liveData, PrefsManager.Kies.LiveData);
		mLive = true;
		CoinGenerator.instance.StartLive ();
		Invoke ("StartLiveAnimation", 3.0f);
	}

	public void ContinueLive (float time) {
		mTime = time;
		if (mLive) {
			return;
		} 
		mLive = true;
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Live);
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.StartLive ();
		}
		EntranceStageManager.instance.StartLive ();
		ScoutStageManager.instance.StartLive ();
		livePanelObject.SetActive (true);
		CoinGenerator.instance.StartLive ();
		OpenCurtain ();
	}

	public bool IsLive {
		get {
			return mLive;
		}
	}

	private void StartLiveAnimation () {
		SoundManager.instance.FadeoutSE (SoundManager.SE_CHANNEL.Cheer);
		EntranceStageManager.instance.StartLive ();
		ScoutStageManager.instance.StartLive ();
		logoObject.SetActive (true);
		logoObject.GetComponent<UISprite> ().alpha = 0;
		TweenAlpha.Begin (logoObject, 3.0f, 1f);
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Live);
		Invoke ("OpenCurtain", 12.3f);
		Invoke ("OpenBall", 7.0f);
	}

	public void FinishLive () {
		mLive = false;
		iTweenEvent.GetEvent (mirrorBallSpriteObject, "LiveFinishEvent").Play ();
		iTweenEvent.GetEvent (ballParent, "RotateEvent").Stop ();
		iTweenEvent.GetEvent (spinTextureObject, "LiveStartEvent").Stop ();
		ballParent.transform.localEulerAngles = new Vector3 (0, 0, 0);
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.FinishLive ();
		}
		//ライブデータをリセット
		LiveData liveData = new LiveData ();
		PrefsManager.instance.WriteData<LiveData> (liveData, PrefsManager.Kies.LiveData);
		EntranceStageManager.instance.FinishLive ();
		ScoutStageManager.instance.FinishLive ();
		CoinGenerator.instance.FinishLive ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
	}

	private void OpenCurtain () {
		logoObject.SetActive (false);
		FenceManager.instance.HideFence ();
		foreach (GameObject curtain in curtainArray) {
			iTweenEvent.GetEvent (curtain, "OpenEvent").Play ();
		}
		foreach (GameObject ball in ballArray) {
			ball.SetActive (false);
		}
		iTweenEvent.GetEvent (spinTextureObject, "LiveStartEvent").Play ();
		iTweenEvent.GetEvent (mirrorBallSpriteObject, "LiveStartEvent").Play ();
		CoinGenerator.instance.StartGenerating ();
	}

	private void OpenBall () {
		for (int i = 0; i < ballArray.Length; i++) {
			GameObject ball = ballArray [i];
			if (!ball.activeSelf) {
				ball.SetActive (true);
				if (i == 4) {
					iTweenEvent.GetEvent (ballParent, "RotateEvent").Play ();
					iTweenEvent.GetEvent (ballParent, "ExitEvent").Play ();
					foreach (GameObject ballObject in ballArray) {
						iTweenEvent.GetEvent (ballObject, "CenterEvent").Play ();
					}
				}
				break;
			}
		}
		bool a = false;
		for (int i = 0; i < ballArray.Length; i++) {
			if (a) {
				break;
			}
			GameObject ball = ballArray [i];
			if (!ball.activeSelf) {
				switch (i) {
				case 1:
					a = true;
					Invoke ("OpenBall", 1.4f);
					break;
				case 2:
					a = true;
					Invoke ("OpenBall", 1.4f);
					break;
				case 3:
					a = true;
					Invoke ("OpenBall", 0.8f);
					break;
				case 4:
					a = true;
					Invoke ("OpenBall", 0.7f);
					break;
				}
			}
		}

	}

}
