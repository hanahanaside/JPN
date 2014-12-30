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
	}


	#endif
}
