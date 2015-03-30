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
		float right = 150f;
		float top = 0f;
		float left = 0f;
		float botton = 20f;
		float topDifference = 50f;
		#if !UNITY_EDITOR
		top = 100f;
		right = 200f;
		botton = 60f;
		left = 10f;
		topDifference = 90f;
		#endif

		#if UNITY_EDITOR
		Rect pauseRect = new Rect (left, top += topDifference, right, botton);
		bool clickedPause = GUI.Button (pauseRect, "save prefs");
		if (clickedPause) {
			PlayerDataKeeper.instance.SaveData ();
		}
		#endif
		Rect prefsRect = new Rect (left, top += topDifference, right, botton);
		bool clickedPrefs = GUI.Button (prefsRect, "セーブデータを削除");
		if (clickedPrefs) {
			Debug.Log ("セーブデータを削除");
			PlayerPrefs.DeleteAll ();
		}

		Rect deleteDBRect = new Rect (left, top += topDifference, right, botton);
		bool clickedDeleteDB = GUI.Button (deleteDBRect, "全ステージを削除");
		if (clickedDeleteDB) {
			DatabaseHelper.instance.DeleteDB ();
			DatabaseHelper.instance.CopyDB ();
		}

		Rect releaseAllStageRect = new Rect (left, top += topDifference, right, botton);
		bool clickedReleaseAllStage = GUI.Button (releaseAllStageRect, "全ステージ解放");
		if (clickedReleaseAllStage) {
			Debug.Log ("全ステージ解放");
			StageDao dao = DaoFactory.CreateStageDao ();
			for (int i = 1; i <= 48; i++) {
				StageData stage = new StageData ();
				stage.Id = i;
				stage.IdolCount = 23;
				stage.FlagConstruction = StageData.NOT_CONSTRUCTION;
				stage.UpdatedDate = System.DateTime.Now.ToString ();
				dao.UpdateRecord (stage);
			}
		}

		Rect unlockAllAreaRect = new Rect (left, top += topDifference, right, botton);
		bool clickedUnlockArea = GUI.Button (unlockAllAreaRect, "全エリアロック解除");
		if (clickedUnlockArea) {
			int[] clearedPuzzleCountArray = { 1, 1, 1, 1, 1, 1, 1, 1 };
			PrefsManager.instance.ClearedPuzzleCountArray = clearedPuzzleCountArray;
		}

		Rect decreaseCoinRect = new Rect (left, top += topDifference, right, botton);
		bool decreaseCoin = GUI.Button (decreaseCoinRect, "コインを1000減らす");
		if (decreaseCoin) {
			PlayerDataKeeper.instance.DecreaseCoinCount (1000);
		}

		Rect increaseTicketRect = new Rect (left, top += topDifference, right, botton);
		bool increaseTicket = GUI.Button (increaseTicketRect, "チケットを100枚増やす");
		if (increaseTicket) {
			PlayerDataKeeper.instance.IncreaseTicketCount (100);
		}

	}
}
