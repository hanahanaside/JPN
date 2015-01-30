using UnityEngine;
using System.Collections;
using System;

public class PuzzleTutorialManager : MonoSingleton<PuzzleTutorialManager> {

	public GameObject[] arrowObjectArray;
	public UISprite[] targetSpriteArray;
	public UILabel remainingTapCountLabel;
	public GameObject natsumotoObject;
	public UILabel tutorialLabel;
	public TypewriterEffect typeWriterEffect;
	public GameObject okButtonObject;
	private Entity_tutorial mEntityTutorial;
	private int mRemainingTapCount = 8;
	private int mTutorialMessageIndex = 8;
	private int mTutorialIndex;
	private int mCompleteCount;

	void OnEnable () {
		RefereeTutorial.UpdateGameEvent += UpdateGameEvent;
		TargetTutorial.UpdateGameEvent += UpdateGameEvent;
		TargetTutorial.CompleteTargetEvent += CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent += UpdateGameEvent;
		MapDialogManagerTutorial.mapClosedEvent += MapClosedEvent;
	}

	void OnDisable () {
		RefereeTutorial.UpdateGameEvent -= UpdateGameEvent;
		TargetTutorial.UpdateGameEvent -= UpdateGameEvent;
		TargetTutorial.CompleteTargetEvent -= CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent -= UpdateGameEvent;
		MapDialogManagerTutorial.mapClosedEvent -= MapClosedEvent;
	}

	void Start () {
		PlayerDataKeeper.instance.Init ();
		mEntityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		natsumotoObject.SetActive (true);
		FenceManager.instance.ShowFence ();
		iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
	}

	void Update () {
		remainingTapCountLabel.text = "残り" + mRemainingTapCount + "タップ";
	}

	void CompleteShowEvent () {
		okButtonObject.SetActive (false);
		UpdateMessage ();
	}

	void CompleteHideEvent () {
		natsumotoObject.transform.localPosition = new Vector3 (0, 0, 0);
		natsumotoObject.SetActive (false);
		if(mTutorialMessageIndex == 10){
			mTutorialIndex++;
			arrowObjectArray [mTutorialIndex].SetActive (true);
			targetSpriteArray [mTutorialIndex].depth = 2;
		}
	}

	void MapClosedEvent () {
		mTutorialIndex++;
		arrowObjectArray [mTutorialIndex].SetActive (true);
		targetSpriteArray [mTutorialIndex].depth = 2;
	}

	//ゲームを更新する
	void UpdateGameEvent () {
		mRemainingTapCount--;
		switch (mTutorialIndex) {
		case 4:
			mTutorialMessageIndex++;
			natsumotoObject.SetActive (true);
			FenceManager.instance.ShowFence ();
			iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
			arrowObjectArray [mTutorialIndex].SetActive (false);
			break;
		case 8:
			arrowObjectArray [mTutorialIndex].SetActive (false);
			mTutorialMessageIndex++;
			FenceManager.instance.ShowFence ();
			natsumotoObject.SetActive (true);
			iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
			break;
		default:
			arrowObjectArray [mTutorialIndex].SetActive (false);
			mTutorialIndex++;
			arrowObjectArray [mTutorialIndex].SetActive (true);
			targetSpriteArray [mTutorialIndex].depth = 2;
			break;
		}
	}

	void CompleteTargetEvent (string targetTag) {
		string id = targetTag.Remove (0, 5);
		CharacterVoiceManager.instance.PlayVoice (Convert.ToInt32 (id) -1);
		GetIdleDialogManager.instance.Show (Convert.ToInt32 (id));
	}

	public void OnMessageFinished(){
		okButtonObject.SetActive (true);
	}

	public void OKButtonClicked () {
		okButtonObject.SetActive (false);
		tutorialLabel.text = "";
		Debug.Log ("index " + mTutorialMessageIndex);
		switch (mTutorialMessageIndex) {
		case 8:
			mTutorialMessageIndex++;
			UpdateMessage ();
			break;
		case 9:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			arrowObjectArray [mTutorialIndex].SetActive (true);
			targetSpriteArray [mTutorialIndex].depth = 2;
			break;
		case 10:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			break;
		case 11:
			Application.LoadLevel ("MainTutorial");
			break;
		}

		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void TargetClicked () {
		arrowObjectArray [mTutorialIndex].SetActive (false);
		targetSpriteArray [mTutorialIndex].depth = 0;
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	private void UpdateMessage () {
		typeWriterEffect.ResetToBeginning ();
		tutorialLabel.text = mEntityTutorial.param [mTutorialMessageIndex].message;
	}
}
