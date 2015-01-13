using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Referee : MonoBehaviour {

	public static event Action UpdateGameEvent;

	public UIGrid targetGrid;
	public GameObject openEffectPrefab;
	private List<GameObject> mTargetObjectList;

	void OnEnable () {
		Puzzle.OpenedPuzzleEvent += OpenedPuzzleEvent;
		PuzzleTable.CreatedPuzzleTableEvent += CreatedPuzzleTableEvent;
	}

	void OnDisable () {
		Puzzle.OpenedPuzzleEvent -= OpenedPuzzleEvent;
		PuzzleTable.CreatedPuzzleTableEvent -= CreatedPuzzleTableEvent;
	}

	//パズルテーブルが作られた時に呼ばれる
	void CreatedPuzzleTableEvent (GameObject[] puzzleObjectArray) {
		mTargetObjectList = new List<GameObject> ();
		foreach (GameObject puzzleObject in puzzleObjectArray) {
			string tag = "";
			if (puzzleObject.tag == "ticket") {
				tag = "Ticket";
			} else {
				tag = puzzleObject.tag.Remove (0, 5);
			}

			GameObject targetPrefab = Resources.Load ("Target/Target_" + tag) as GameObject;
			GameObject targetObject = Instantiate (targetPrefab) as GameObject;
			targetGrid.AddChild (targetObject.transform);
			targetObject.transform.localScale = new Vector3 (1, 1, 1);
			mTargetObjectList.Add (targetObject);
		}
		targetGrid.repositionNow = true;
	}
		
	//パズルオープン時に呼ばれる
	void OpenedPuzzleEvent (GameObject puzzleObject) {
		string tag = puzzleObject.tag;
		Debug.Log ("tag " + tag);
		switch (tag) {
		case "blank":
			UpdateGameEvent ();
			break;
		case "coin_1":
			PlayerDataKeeper.instance.IncreaseCoinCount (1);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			UpdateGameEvent ();
			break;
		case "coin_5":
			PlayerDataKeeper.instance.IncreaseCoinCount (5);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			UpdateGameEvent ();
			break;
		case "coin_10":
			PlayerDataKeeper.instance.IncreaseCoinCount (10);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			UpdateGameEvent ();
			break;
		case "coin_100":
			PlayerDataKeeper.instance.IncreaseCoinCount (100);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			UpdateGameEvent ();
			break;
		case "coin_1000":
			PlayerDataKeeper.instance.IncreaseCoinCount (1000);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			UpdateGameEvent ();
			break;
		case "ticket":
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.TradeIdol);
			PlayerDataKeeper.instance.IncreaseTicketCount (1);
			UpdateGameEvent ();
			break;
		}
		foreach (GameObject targetObject in mTargetObjectList) {
			if (targetObject == null) {
				continue;
			}
			string targetTag = targetObject.tag;
			//ターゲットと違う場合はコンティニュー
			if (tag != targetTag) {
				continue;
			}
			Target target = targetObject.GetComponent<Target> ();
			target.Correct ();
		}
	}
}
