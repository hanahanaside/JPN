using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Referee : MonoBehaviour {

	public UILabel remainingTapCountLabel;
	public UIGrid targetGrid;
	private List<GameObject> mTargetObjectList;
	private int mRemainingTapCount = 10;

	void OnEnable () {
		Puzzle.OpenedPuzzleEvent += OpenedPuzzleEvent;
		Target.CompleteTargetEvent += CompleteTargetEvent;
	}

	void OnDisable () {
		Puzzle.OpenedPuzzleEvent -= OpenedPuzzleEvent;
		Target.CompleteTargetEvent -= CompleteTargetEvent;
	}

	void Update(){
		remainingTapCountLabel.text = "残りタップ" + mRemainingTapCount + "回";
	}

	//アイドルゲットダイアログを閉じた時に呼ばれる
	void ClosedDialogEvent (string dialogTag) {
		FenceManager.instance.HideFence ();
	}

	//パズル完成時に呼ばれる
	void CompleteTargetEvent (string targetTag) {
		string id = targetTag.Remove (0, 7);
		FenceManager.instance.ShowFence ();
		GameObject getIdleDialogPrefab = Resources.Load ("Dialog/GetIdleDialog_" + id) as GameObject;
		GameObject getIdleDialogObject = Instantiate (getIdleDialogPrefab) as GameObject;
		getIdleDialogObject.transform.parent = transform.parent.transform.parent;
		getIdleDialogObject.transform.localScale = new Vector3 (1, 1, 1);
		GetIdleDialogManager.instance.IdleID = Convert.ToInt32 (id);
	}

	void Start () {
		mTargetObjectList = new List<GameObject> ();
		foreach (string tag in PuzzleTable.instance.puzzleTag) {
			GameObject targetPrefab = Resources.Load ("Target/Target_" + tag) as GameObject;
			GameObject targetObject = Instantiate (targetPrefab) as GameObject;
			targetGrid.AddChild (targetObject.transform);
			targetObject.transform.localScale = new Vector3 (1, 1, 1);
			mTargetObjectList.Add (targetObject);
		}
	}

	void OpenedPuzzleEvent (string tag) {
		mRemainingTapCount--;
		switch (tag) {
		case "puzzle_coin_1":
			PlayerDataKeeper.instance.IncreaseCoinCount (1);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_coin_5":
			PlayerDataKeeper.instance.IncreaseCoinCount (5);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_coin_10":
			PlayerDataKeeper.instance.IncreaseCoinCount (10);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_coin_100":
			PlayerDataKeeper.instance.IncreaseCoinCount (100);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_ticket":
			PlayerDataKeeper.instance.IncreaseTicketCount (1);
			break;
		}
		foreach (GameObject targetObject in mTargetObjectList) {
			CheckAnswer (tag, targetObject);
		}
	}


	private void CheckAnswer (string tag, GameObject targetObject) {
		string targetTag = targetObject.tag;
		if (tag != targetTag) {
			return;
		}
		Target target = targetObject.GetComponent<Target> ();
		target.Correct ();
		if (target.CheckNotComplete()) {

		} else {
			target.StartCompleteEvent ();
		}

	}

	void CompleteExitEvent () {
		FenceManager.instance.ShowFence ();
	}

}
