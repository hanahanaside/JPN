﻿using UnityEngine;
using System.Collections;

public class SplashSceneManager : MonoBehaviour {

	public GameObject fadeoutObject;

	void OnEnable(){
		DatabaseHelper.CreatedDatabaseEvent += CreatedDatabaseEvent;
	}

	void OnDisable(){
		DatabaseHelper.CreatedDatabaseEvent -= CreatedDatabaseEvent;
	}

	void Start () {
		DatabaseHelper.instance.CreateDB ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Hanauta);
	}

	void CreatedDatabaseEvent(){
		fadeoutObject.SetActive (true);
	}

	public void FinishedFadeoutEvent(){
//		StageDao dao = DaoFactory.CreateStageDao ();
//		for(int i = 1;i <= 5;i++){
//			StageData stage = new StageData ();
//			stage.Id = i;
//			stage.IdolCount = 20;
//			stage.FlagConstruction = StageData.NOT_CONSTRUCTION;
//		//	stage.UpdatedDate = System.DateTime.Now.ToString ();
//			dao.UpdateRecord (stage);
//		}
		Transition ();
	}

	private void Transition(){
		if(PrefsManager.instance.TutorialFinished){
			LoadLevelName.instance.loadLevelName = "Main";
			Application.LoadLevel ("Loading");
		}else {
//			LoadLevelName.instance.loadLevelName = "Main";
//			Application.LoadLevel ("Loading");
			PlayerPrefs.DeleteAll ();
			Application.LoadLevel ("MainTutorial");
		}
	}
}
