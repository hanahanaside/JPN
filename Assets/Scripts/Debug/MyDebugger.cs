using UnityEngine;
using System.Collections;
using System;

public class MyDebugger : MonoBehaviour {

	public GameObject soundManagerPrefab;

	#if UNITY_EDITOR
	void Awake () {
		if (GameObject.FindGameObjectWithTag ("SoundManager") == null) {
			Instantiate (soundManagerPrefab);
		}
	}

	void OnGUI () {
		Rect pauseRect = new Rect (10, 30, 80, 20);
		bool clickedPause = GUI.Button (pauseRect, "save data");
		if (clickedPause) {
			OnPauseButtonClicked ();
		}
		Rect deleteDBRect = new Rect (10, 60, 80, 20);
		bool clickedDeleteDB = GUI.Button (deleteDBRect, "delete DB");
		if (clickedDeleteDB) {
			DatabaseHelper.instance.DeleteDB ();
			DatabaseHelper.instance.CopyDB ();
		}
		Rect prefsRect = new Rect (10, 90, 80, 20);
		bool clickedPrefs = GUI.Button (prefsRect, "clear prefs");
		if (clickedPrefs) {
			PrefsManager.instance.PlayerDataJson = "";
		}
	}

	private void OnPauseButtonClicked () {
		string sceneName = Application.loadedLevelName;
		if (sceneName == "Main") {
			MainSceneManager.instance.OnApplicationPause (true);
		}
		if(sceneName == "Puzzle"){

		}
	}


	#endif
}
