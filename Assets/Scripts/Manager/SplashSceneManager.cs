using UnityEngine;
using System.Collections;

public class SplashSceneManager : MonoBehaviour {

	public bool debug;
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
		Transition ();
	}

	private void Transition(){
		if(PrefsManager.instance.TutorialFinished){
			LoadLevelName.instance.loadLevelName = "Main";
			Application.LoadLevel ("Loading");
		}else {
			PlayerPrefs.DeleteAll ();
			Application.LoadLevel ("MainTutorial");
		}
	}
}
