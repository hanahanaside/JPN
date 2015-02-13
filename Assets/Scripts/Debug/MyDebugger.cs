using UnityEngine;
using System.Collections;
using System;

public class MyDebugger : MonoBehaviour {

	public GameObject soundManagerPrefab;
	public GameObject characterVoiceManagerPrefab;

	#if UNITY_EDITOR
	void Awake () {
		if (GameObject.FindGameObjectWithTag ("SoundManager") == null) {
			Instantiate (soundManagerPrefab);
			Instantiate (characterVoiceManagerPrefab);
		}
	}

	void OnGUI () {
		string sceneName = Application.loadedLevelName;
		if (sceneName != "Puzzle") {
			ShowMainGUI ();
		}
	}

	private void ShowMainGUI () {
		Rect pauseRect = new Rect (10, 30, 80, 20);
		bool clickedPause = GUI.Button (pauseRect, "save pref");
		if (clickedPause) {
			PlayerDataKeeper.instance.SaveData ();
		}
		Rect deleteDBRect = new Rect (10, 60, 80, 20);
		bool clickedDeleteDB = GUI.Button (deleteDBRect, "reset DB");
		if (clickedDeleteDB) {
			DatabaseHelper.instance.DeleteDB ();
			DatabaseHelper.instance.CopyDB ();
		}
		Rect prefsRect = new Rect (10, 90, 80, 20);
		bool clickedPrefs = GUI.Button (prefsRect, "clear prefs");
		if (clickedPrefs) {
			PlayerPrefs.DeleteAll ();
		}

		Rect releaseAllStageRect = new Rect (10, 120, 80, 20);
		bool clickedReleaseAllStage = GUI.Button (releaseAllStageRect, "release All Stage");
		if(clickedReleaseAllStage){
			StageDao dao = DaoFactory.CreateStageDao ();
			for(int i = 1;i <= 40;i++){
				StageData stage = new StageData ();
				stage.Id = i;
				stage.IdolCount = 20;
				stage.FlagConstruction = StageData.NOT_CONSTRUCTION;
				stage.UpdatedDate = System.DateTime.Now.ToString ();
				dao.UpdateRecord (stage);
			}
		}
	}


	#endif
}
