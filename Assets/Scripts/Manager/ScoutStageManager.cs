﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoutStageManager : StageManagerBase {

	public Transform[] areaPositionArray;
	public GameObject dartsObject;
	public GameObject planeObject;
	public GameObject fadeOutSpriteObject;
	public GameObject goScoutButtonObject;
	public UILabel costLabel;
	private int mCost;
	private List<Fan> mFanList = new List<Fan> ();
	private static ScoutStageManager sInstance;

	public static bool FlagScouting{ get; set; }

	public static int SelectedAreaId{ get; set; }

	void OnEnable () {
		AreaPanelManager.instance.OnAreaClickedEvent += OnAreaClickedEvent; 
	}

	void OnDisable () {
		AreaPanelManager.instance.OnAreaClickedEvent -= OnAreaClickedEvent;
	}

	void Awake () {
		sInstance = this;
		if (SelectedAreaId == 0) {
			SelectedAreaId = 1;
		}
		dartsObject.transform.localPosition = areaPositionArray [SelectedAreaId - 1].localPosition;
		dartsObject.SetActive (true);
		mCost = AreaCostCaluculator.instance.CalcCost (SelectedAreaId - 1);
		costLabel.text = "" + mCost;

		for (int i = 0; i < 20; i++) {
			int rand = UnityEngine.Random.Range (1, 14);
			GameObject fanPrefab = Resources.Load ("Model/Fan/Fan_" + rand) as GameObject;
			GameObject fanObject = Instantiate (fanPrefab) as GameObject;
			float x = UnityEngine.Random.Range (-250.0f, 250.0f);
			float y = UnityEngine.Random.Range (-220.0f, -160.0f);
			fanObject.transform.parent = transform;
			fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			fanObject.transform.localPosition = new Vector3 (x, y, 0);
			mFanList.Add (fanObject.GetComponent<Fan> ());
			fanObject.GetComponent<Fan> ().Init ();
		}
	}


	void OnAreaClickedEvent (int areaIndexNumber) {
		mCost = AreaCostCaluculator.instance.CalcCost (areaIndexNumber);
		SelectedAreaId = areaIndexNumber + 1;
		dartsObject.transform.localPosition = areaPositionArray [areaIndexNumber].localPosition;
		dartsObject.SetActive (true);
		costLabel.text = "" + mCost; 
	}

	public static ScoutStageManager instance{
		get{
			return sInstance;
		}
	}

	void OnPlaneEventCompleted () {
		fadeOutSpriteObject.SetActive (true);
	}

	public void OnFadeOutFinished () {
		Application.LoadLevel ("Puzzle");
	}

	public void OnAreaButtonClicked () {
		AreaPanelManager.instance.ShowAreaPanel ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public override void StartLive () {
		foreach(Fan fan in mFanList){
			fan.StartLive ();
		}
	}

	public override void FinishLive(){

	}

	public void OnGoScoutButtonClicked () {
		if (PlayerDataKeeper.instance.CoinCount < mCost) {
			FenceManager.instance.ShowFence ();
			OKDialog.instance.Show ("コインが不足しています");
			return;
		}
		goScoutButtonObject.SetActive (false);
		FlagScouting = true;
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		PlayerDataKeeper.instance.SaveData ();
		LiveManager.instance.Save ();
		iTweenEvent.GetEvent (planeObject, "moveOut").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Plane);
	}

	public void PlayMoveInPlaneAnimation () {
		iTweenEvent.GetEvent (planeObject, "moveIn").Play ();
	}
}
