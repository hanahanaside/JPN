using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RefereeTutorial : MonoBehaviour {

	public static event Action UpdateGameEvent;

	public GameObject openEffectPrefab;
	public TargetTutorial target1;
	public TargetTutorial target2;

	void OnEnable () {
		Puzzle.OpenedPuzzleEvent += OpenedPuzzleEvent;
	}

	void OnDisable () {
		Puzzle.OpenedPuzzleEvent -= OpenedPuzzleEvent;
	}

	//パズルオープン時に呼ばれる
	void OpenedPuzzleEvent (GameObject puzzleObject) {
		string tag = puzzleObject.tag;
		Debug.Log ("tag " +tag);
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
		case "ticket":
			PlayerDataKeeper.instance.IncreaseTicketCount (1);
			UpdateGameEvent ();
			break;
		case "idle_1":
			target1.Correct ();
			break;
		case "idle_2":
			target2.Correct ();
			break;
		}
	}

}
