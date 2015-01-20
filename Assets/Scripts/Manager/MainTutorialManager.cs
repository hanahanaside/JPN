using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainTutorialManager : MonoSingleton<MainTutorialManager> {

	public GameObject natsumotoObject;
	public GameObject scoutArrowObject;
	public GameObject areaArrowObject;
	public GameObject areaDialogObject;
	public GameObject dartsObject;
	public GameObject goScoutArrowObject;
	public GameObject planeObject;
	public GameObject fadeOutSpriteObject;
	public GameObject backGroundTextureObject;
	public GameObject liveArrowObject;
	public GameObject coinSpriteObject;
	public GameObject stagePrefab;
	public GameObject okButtonObject;
	public UICenterOnChild centerOnChild;
	public UIGrid grid;
	private UILabel tutorialLabel;
	private TypewriterEffect typeWriterEffect;
	private Entity_tutorial mEntityTutorial;

	private static int sTutorialIndex;

	void CompleteShowEvent () {
		typeWriterEffect.ResetToBeginning ();
		tutorialLabel.text = mEntityTutorial.param [sTutorialIndex].message;
		okButtonObject.SetActive (false);
	}

	void CompleteHideEvent () {
		natsumotoObject.transform.localPosition = new Vector3 (0, 0, 0);
		natsumotoObject.SetActive (false);
	}

	void CompleteHideAreaDialogEvent () {
		areaDialogObject.SetActive (false);
	}

	void OnPlaneEventCompleted () {
		fadeOutSpriteObject.SetActive (true);
	}

	void SleepEvent(){

	}

	void WakeupEvent(){
	
	}

	public void OnFadeOutFinished () {
		Application.LoadLevel ("PuzzleTutorial");
	}

	void OnEnable(){
		StageManager.SleepEvent += SleepEvent;
		StageManager.WakeupEvent += WakeupEvent;
	}

	void OnDisable(){
		StageManager.SleepEvent -= SleepEvent;
		StageManager.WakeupEvent -= WakeupEvent;
	}

	void Start () {
		if (sTutorialIndex != 0) {
			sTutorialIndex = 12;
			CreateStage ();
			backGroundTextureObject.collider.enabled = true;
		}else {
			backGroundTextureObject.collider.enabled = false;
		}
		List<Transform> childList = grid.GetChildList ();
		centerOnChild.CenterOn (childList [1]);
		PlayerDataKeeper.instance.Init ();
		mEntityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		tutorialLabel = natsumotoObject.transform.FindChild ("Label").GetComponent<UILabel> ();
		typeWriterEffect = natsumotoObject.transform.FindChild ("Label").GetComponent<TypewriterEffect> ();
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
	}

	public void OnMessageFinished(){
		okButtonObject.SetActive (true);
	}

	public void OKButtonClicked () {
		Debug.Log ("index " + sTutorialIndex);
		okButtonObject.SetActive (false);
		switch (sTutorialIndex) {
		case 0:
			sTutorialIndex++;
			UpdateMessage ();
			break;
		case 1:
			sTutorialIndex++;
			UpdateMessage ();
			coinSpriteObject.SetActive (true);
			break;
		case 2:
			PlayerDataKeeper.instance.IncreaseCoinCount (10000);
			sTutorialIndex++;
			UpdateMessage ();
			coinSpriteObject.SetActive (false);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			return;
		case 3:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			scoutArrowObject.SetActive (true);
			StartTweenColor ("A_ScoutButton",new Color(0.7f,0.5f,0.5f,1));
			break;
		case 4:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			areaArrowObject.SetActive (true);
			StartTweenColor ("AreaButton",new Color(0.9f,0.8f,0.8f,1));
			break;
		case 5:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			areaDialogObject.SetActive (true);
			iTweenEvent.GetEvent (areaDialogObject, "ShowEvent").Play ();
			GameObject.Find ("TargetCell").GetComponent<TweenColor>().enabled = true;
			break;
		case 6:
			sTutorialIndex++;
			UpdateMessage ();
			break;
		case 7:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			dartsObject.SetActive (true);
			goScoutArrowObject.SetActive (true);
			StartTweenColor ("GoScoutButton",new Color(0.7f,0.5f,0.5f,1));
			break;
		case 12:
			sTutorialIndex++;
			UpdateMessage ();
			break;
		case 13:
			PlayerDataKeeper.instance.IncreaseTicketCount (10);
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			StartTweenColor ("LiveButton", new Color (0.7f, 0.5f, 0.5f, 1));
			liveArrowObject.SetActive (true);
			break;
		case 14:
			PlayerDataKeeper.instance.SaveData ();
			PrefsManager.instance.TutorialFinished = true;
			ScoutStageManager.FlagScouting = true;
			#if UNITY_IPHONE
			APNsRegister.instance.RegisterForRemoteNotifcations ();
			#endif
			Application.LoadLevel ("Main");
			break;
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void ScoutButtonClicked () {
		if (sTutorialIndex != 3) {
			return;
		}

		List<Transform> childList = grid.GetChildList ();
		centerOnChild.CenterOn (childList [0]);
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
		sTutorialIndex++;
		UpdateMessage ();
		scoutArrowObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void ShowFinishMessage(){
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
		sTutorialIndex++;
		UpdateMessage ();
	}

	public void AreaButtonClicked () {
		if (sTutorialIndex != 4) {
			return;
		}
		areaArrowObject.SetActive (false);
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
		sTutorialIndex++;
		UpdateMessage ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void SugekitaButtonClicked () {
		PlayerDataKeeper.instance.DecreaseCoinCount (500);
		iTweenEvent.GetEvent (areaDialogObject, "HideEvent").Play ();
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject, "ShowEvent").Play ();
		sTutorialIndex++;
		UpdateMessage ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void GoScoutButtonClicked () {
		if (sTutorialIndex != 7) {
			return;
		}
		iTweenEvent.GetEvent (planeObject, "moveOut").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Plane);
		GameObject.Find ("GoScoutButton").SetActive(false);
		goScoutArrowObject.SetActive (false);
		PlayerDataKeeper.instance.DecreaseCoinCount (100);
		PlayerDataKeeper.instance.SaveData ();
	}

	public void LiveButtonClicked(){
		if (sTutorialIndex != 13) {
			return;
		}
		liveArrowObject.SetActive (false);
		UIButton.current.gameObject.SetActive (false);
		LiveManagerTutorial.instance.StartLive (30.0f);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	private void UpdateMessage () {
		typeWriterEffect.ResetToBeginning ();
		tutorialLabel.text = mEntityTutorial.param [sTutorialIndex].message;
	}

	private void CreateStage(){
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		foreach(Stage stage in stageList){
			stage.UpdatedDate = DateTime.Now.ToString ();
			dao.UpdateRecord (stage);
		}
		foreach (Stage stage in stageList) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			grid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
			stageObject.GetComponentInChildren<StageManager> ().Init(stage);
		}
	}

	private void StartTweenColor(string objectName,Color color){
		TweenColor tweenColor = GameObject.Find (objectName).GetComponent<TweenColor>();
		tweenColor.style = UITweener.Style.PingPong;
		tweenColor.to = color;
		tweenColor.PlayForward ();
	}
}
