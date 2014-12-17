using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveManager : MonoSingleton<LiveManager> {

	public GameObject[] curtainArray;
	public GameObject[] ballArray;
	public GameObject label;
	public GameObject ballParent;

	private float mTime;
	private bool mLive;
	private GameObject mirrorBallSpriteObject;
	private GameObject spinTextureObject;
	private Vector3[] mStartCurtainPosition;
	private UILabel mRemainingLiveTimeLabel;
	public GameObject livePanelObject;

	void Awake(){
		mirrorBallSpriteObject = livePanelObject.transform.Find ("MirroBallSprite").gameObject;
		spinTextureObject = livePanelObject.transform.Find ("SpinTexture").gameObject;
		mRemainingLiveTimeLabel = livePanelObject.transform.Find ("RemainingTimeLabel").GetComponent<UILabel>();
		mStartCurtainPosition = new Vector3[2];
		for(int i = 0;i < curtainArray.Length;i++){
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

	void OnCompleteMirrorBallFinishLiveEvent(){
		livePanelObject.SetActive (false);
		for(int i = 0;i < curtainArray.Length;i++){
			curtainArray [i].transform.localPosition = mStartCurtainPosition [i];
		}
		EntranceStageManager.instance.FinishLive ();
	}

	public void StartLive (float time) {
		EntranceStageManager.instance.StartLive ();
		mTime = time;
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.StartLive ();
		}
		livePanelObject.SetActive (true);
		label.SetActive (true);
		mLive = true;
		label.GetComponent<TypewriterEffect> ().ResetToBeginning ();
		label.GetComponent<UILabel> ().text = "会いたくて\n会いたくて\n君に逢いたくて\nこの胸が\nときめくの\n君のせいだから";
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Live);
		Invoke ("OpenCurtain",12.3f);
		Invoke ("OpenBall",7.0f);
	}

	private void FinishLive(){
		mLive = false;
		iTweenEvent.GetEvent (mirrorBallSpriteObject,"LiveFinishEvent").Play();
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		foreach (StageManager stageManager in stageManagerList) {
			stageManager.FinishLive ();
		}
		EntranceStageManager.instance.FinishLive ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
	}

	private void OpenCurtain(){
		foreach(GameObject curtain in curtainArray){
			iTweenEvent.GetEvent (curtain,"OpenEvent").Play();
		}
		foreach (GameObject ball in ballArray) {
			ball.SetActive (false);
		}
		label.SetActive (false);
		iTweenEvent.GetEvent (spinTextureObject,"LiveStartEvent").Play();
		iTweenEvent.GetEvent (mirrorBallSpriteObject,"LiveStartEvent").Play();
	}

	private void OpenBall(){
		for( int i = 0; i< ballArray.Length;i ++ ){
			GameObject ball = ballArray[i];
			if(!ball.activeSelf){
				ball.SetActive (true);
				if(i == 4){
					iTweenEvent.GetEvent (ballParent,"RotateEvent").Play();
					iTweenEvent.GetEvent (ballParent,"ExitEvent").Play();
					foreach(GameObject ballObject in ballArray){
						iTweenEvent.GetEvent (ballObject,"CenterEvent").Play();
					}
				}
				break;
			}
		}
		bool a = false;
		for(int i = 0;i < ballArray.Length;i++){
			if(a){
				break;
			}
			GameObject ball = ballArray [i];
			if(!ball.activeSelf){
				switch(i){
				case 1:
					a = true;
					Invoke ("OpenBall",1.4f);
					break;
				case 2:
					a = true;
					Invoke ("OpenBall",1.4f);
					break;
				case 3:
					a = true;
					Invoke ("OpenBall",0.8f);
					break;
				case 4:
					a = true;
					Invoke ("OpenBall",0.7f);
					break;
				}
			}
		}

	}

}
