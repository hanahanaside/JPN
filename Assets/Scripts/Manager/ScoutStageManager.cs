using UnityEngine;
using System.Collections;
using System;

public class ScoutStageManager : MonoSingleton<ScoutStageManager> {

	public Transform[] areaPositionArray;
	public GameObject dartsObject;
	public GameObject planeObject;
	public GameObject fadeOutSpriteObject;
	public GameObject goScoutButtonObject;
	public UILabel costLabel;
	private int mCost;

	public static bool FlagScouting{ get; set; }

	public static int SelectedAreaId{ get; set; }

	void OnEnable () {
		AreaPanelManager.instance.OnAreaClickedEvent += OnAreaClickedEvent; 
	}

	void OnDisable () {
		AreaPanelManager.instance.OnAreaClickedEvent -= OnAreaClickedEvent;
	}

	void Awake () {
		if (SelectedAreaId == 0) {
			SelectedAreaId = 1;
		}
		dartsObject.transform.localPosition = areaPositionArray [SelectedAreaId - 1].localPosition;
		dartsObject.SetActive (true);
		mCost = AreaCostCaluculator.instance.CalcCost (SelectedAreaId - 1);
		costLabel.text = "" + mCost;
		
	}


	void OnAreaClickedEvent (int areaIndexNumber) {
		mCost = AreaCostCaluculator.instance.CalcCost (areaIndexNumber);
		SelectedAreaId = areaIndexNumber + 1;
		dartsObject.transform.localPosition = areaPositionArray [areaIndexNumber].localPosition;
		dartsObject.SetActive (true);
		costLabel.text = "" + mCost; 
	}

	void OnPlaneEventCompleted () {
		fadeOutSpriteObject.SetActive (true);
	}

	public void StartLive () {
		goScoutButtonObject.SetActive (false);
	}

	public void FinishLive () {
		goScoutButtonObject.SetActive (true);
	}

	public void OnFadeOutFinished () {
		PlayerDataKeeper.instance.ExitDate = DateTime.Now.ToString ();
		Application.LoadLevel ("Puzzle");
	}

	public void OnAreaButtonClicked () {
		AreaPanelManager.instance.ShowAreaPanel ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnGoScoutButtonClicked () {
		if (!dartsObject.activeSelf) {
			return;
		}
		goScoutButtonObject.SetActive (false);
		FlagScouting = true;
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		iTweenEvent.GetEvent (planeObject, "moveOut").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Plane);
	}

	public void PlayMoveInPlaneAnimation () {
		iTweenEvent.GetEvent (planeObject, "moveIn").Play ();
	}
}
