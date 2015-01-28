using UnityEngine;
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
		if(PrefsManager.instance.TutorialFinished){
			Application.LoadLevel ("Main");
		}else {
			Application.LoadLevel ("MainTutorial");
		}
	}
}
