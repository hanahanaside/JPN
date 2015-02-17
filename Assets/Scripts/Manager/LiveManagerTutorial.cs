using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveManagerTutorial : MonoSingleton<LiveManagerTutorial> {

	public GameObject[] curtainArray;
	public GameObject[] ballArray;
	public GameObject ballParent;
	public GameObject curtainHeadObject;
	public GameObject logoObject;
	public GameObject backGroundTextureObject;
	public CoinGenerator coinGenerator;
	public UIGrid grid;

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
		mRemainingLiveTimeLabel.text = "" + (int)mTime;
		if (mTime > 0) {
			return;
		}
		FinishLive ();
	}
		
	void OpenedCurtainEvent () {
		curtainHeadObject.SetActive (false);
	}

	void OnCompleteMirrorBallFinishLiveEvent () {
		livePanelObject.SetActive (false);
		MainTutorialManager.instance.ShowFinishMessage ();
	}

	public void StartLive (float time) {
		mTime = time;
		FenceManager.instance.ShowFence ();
		ballParent.transform.localPosition = new Vector3 (0, 0, 0);
		foreach (GameObject ballObject in ballArray) {
			ballObject.transform.localPosition = new Vector3 (0, 0, 0);
		}
		livePanelObject.SetActive (true);
		curtainHeadObject.SetActive (true);
		spinTextureObject.transform.localEulerAngles = new Vector3 (0, 0, 0);
		SoundManager.instance.StopBGM ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Cheer);
		Invoke ("StartLiveAnimation", 3.0f);
	}

	private void StartLiveAnimation () {
		SoundManager.instance.FadeoutSE (SoundManager.SE_CHANNEL.Cheer);
		List<Transform> childList = grid.GetChildList ();
		for (int i = 2; i < 4; i++) {
			childList [i].BroadcastMessage ("StartLive");
		}
		logoObject.SetActive (true);
		logoObject.GetComponent<UISprite> ().alpha = 0;
		TweenAlpha.Begin (logoObject, 3.0f, 1f);
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Live);
		backGroundTextureObject.collider.enabled = true;
		Invoke ("OpenCurtain", 12.3f);
		Invoke ("OpenBall", 7.0f);
	}

	private void FinishLive () {
		mLive = false;
		iTweenEvent.GetEvent (mirrorBallSpriteObject, "LiveFinishEvent").Play ();
		iTweenEvent.GetEvent (ballParent, "RotateEvent").Stop ();
		iTweenEvent.GetEvent (spinTextureObject, "LiveStartEvent").Stop ();
		ballParent.transform.localEulerAngles = new Vector3 (0, 0, 0);
		List<Transform> childList = grid.GetChildList ();
		for (int i = 2; i < 4; i++) {
			childList [i].BroadcastMessage ("FinishLive");
		}
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
		mLive = true;
		coinGenerator.gameObject.SetActive (true);
		coinGenerator.StartLive ();
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
