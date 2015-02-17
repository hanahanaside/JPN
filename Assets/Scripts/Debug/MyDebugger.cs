using UnityEngine;
using System.Collections;
using System;

public class MyDebugger : MonoBehaviour {

	public GameObject soundManagerPrefab;
	public GameObject characterVoiceManagerPrefab;

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
		float right = 50f;
		float top = 50f;
		float left = 10f;
		float botton = 20f;
		float topDifference = 30f;
		#if !UNITY_EDITOR
		top = 100f;
		right = 200f;
		botton = 60f;
		topDifference = 90f;
		#endif

		#if UNITY_EDITOR
		Rect pauseRect = new Rect (left, top += topDifference, right, botton);
		bool clickedPause = GUI.Button (pauseRect, "save prefs");
		if (clickedPause) {
			PlayerDataKeeper.instance.SaveData ();
		}
		#endif
		Rect prefsRect =  new Rect (left, top += topDifference, right, botton);
		bool clickedPrefs = GUI.Button (prefsRect, "セーブデータを削除");
		if (clickedPrefs) {
			PlayerPrefs.DeleteAll ();
		}



		Rect deleteDBRect =  new Rect (left, top += topDifference, right, botton);
		bool clickedDeleteDB = GUI.Button (deleteDBRect, "全ステージを削除");
		if (clickedDeleteDB) {
			DatabaseHelper.instance.DeleteDB ();
			DatabaseHelper.instance.CopyDB ();
		}

		Rect releaseAllStageRect =  new Rect (left, top += topDifference, right, botton);
		bool clickedReleaseAllStage = GUI.Button (releaseAllStageRect, "全ステージ解放");
		if(clickedReleaseAllStage){
			StageDao dao = DaoFactory.CreateStageDao ();
			for(int i = 1;i <= 47;i++){
				StageData stage = new StageData ();
				stage.Id = i;
				stage.IdolCount = 20;
				stage.FlagConstruction = StageData.NOT_CONSTRUCTION;
				stage.UpdatedDate = System.DateTime.Now.ToString ();
				dao.UpdateRecord (stage);
			}
		}

	}
}
