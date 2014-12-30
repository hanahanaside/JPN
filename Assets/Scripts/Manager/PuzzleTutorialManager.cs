using UnityEngine;
using System.Collections;
using System;

public class PuzzleTutorialManager : MonoSingleton<PuzzleTutorialManager> {

	public PuzzleTableTutorial puzzleTableTutorial;
	public UILabel remainingTapCountLabel;
	public GameObject natsumotoObject;
	public GameObject targetArrowObject;
	public UILabel tutorialLabel;
	public TypewriterEffect typeWriterEffect;
	private Entity_tutorial mEntityTutorial;
	private int mRemainingTapCount = 7;
	private int mTutorialIndex = 8;
	private int mCompleteCount;

	void OnEnable () {
		RefereeTutorial.UpdateGameEvent += UpdateGameEvent;
		TargetTutorial.UpdateGameEvent += UpdateGameEvent;
		TargetTutorial.CompleteTargetEvent += CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent += UpdateGameEvent;
	}

	void OnDisable(){
		RefereeTutorial.UpdateGameEvent -= UpdateGameEvent;
		TargetTutorial.UpdateGameEvent -= UpdateGameEvent;
		TargetTutorial.CompleteTargetEvent -= CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent -= UpdateGameEvent;
	}

	void Start(){
		PlayerDataKeeper.instance.Init ();
		mEntityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		puzzleTableTutorial.CreateTable ();
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
	}

	void Update(){
		remainingTapCountLabel.text = "残り" + mRemainingTapCount + "タップ";
	}

	void CompleteShowEvent(){
		UpdateMessage ();
	}

	void CompleteHideEvent(){
		natsumotoObject.transform.localPosition = new Vector3 (0,0,0);
		natsumotoObject.SetActive (false);
	}

	//ゲームを更新する
	void UpdateGameEvent () {
		mRemainingTapCount--;
		if(mRemainingTapCount <= 0){
			mTutorialIndex++;
			natsumotoObject.SetActive (true);
			iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		}
	}

	void CompleteTargetEvent (string targetTag) {
		string id = targetTag.Remove (0, 5);
		GetIdleDialogManager.instance.Show (Convert.ToInt32 (id));
		mCompleteCount++;
		if(mCompleteCount == 1){
			mTutorialIndex++;
			natsumotoObject.SetActive (true);
			iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		}
	}

	public void OKButtonClicked(){
		switch(mTutorialIndex){
		case 8:
			mTutorialIndex++;
			UpdateMessage ();
			break;
		case 9:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			targetArrowObject.SetActive (true);
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

	public void TargetClicked(){
		targetArrowObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	private void UpdateMessage(){
		typeWriterEffect.ResetToBeginning ();
		tutorialLabel.text = mEntityTutorial.param [mTutorialIndex].message;
	}
}
