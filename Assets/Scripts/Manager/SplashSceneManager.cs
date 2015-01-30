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
		StageDao dao = DaoFactory.CreateStageDao ();
		for(int i = 1;i <= 40;i++){
			Stage stage = new Stage ();
			stage.Id = i;
			stage.IdleCount = 20;
			stage.FlagConstruction = Stage.NOT_CONSTRUCTION;
			stage.UpdatedDate = System.DateTime.Now.ToString ();
			dao.UpdateRecord (stage);
		}
		if(PrefsManager.instance.TutorialFinished){
			Application.LoadLevel ("Main");
		}else {
			Application.LoadLevel ("MainTutorial");
		}
	}
}
