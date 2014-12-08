using UnityEngine;
using System.Collections;
using System;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	public UILabel remainingTapCountLabel;
	public UIGrid targetGrid;
	public Transform puzzleTableTransform;
	private int mRemainingTapCount = 10;
	private int mUpdateCount;

	void OnEnable () {
		Target.CompletePuzzleEvent += CompletePuzzleEvent;
		Target.UpdateGameEvent += UpdateGameEvent;
		GetIdleDialogManager.ClosedEvent += ClosedDialogEvent;
		FinishPuzzleDialogManager.FinishPuzzleEvent += FinishPuzzleEvent;
	}

	void OnDisable () {
		Target.CompletePuzzleEvent -= CompletePuzzleEvent;
		Target.UpdateGameEvent -= UpdateGameEvent;
		GetIdleDialogManager.ClosedEvent -= ClosedDialogEvent;
		FinishPuzzleDialogManager.FinishPuzzleEvent -= FinishPuzzleEvent;
	}

	void Start () {
		PlayerDataKeeper.instance.Init ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
		remainingTapCountLabel.text = "残りタップ" + mRemainingTapCount + "回";
		GameObject puzzleTablePrefab = Resources.Load ("Table_1/PuzzleTable_3") as GameObject;
		GameObject puzzleTableObject = Instantiate (puzzleTablePrefab)as GameObject;
		puzzleTableObject.transform.parent = puzzleTableTransform;
		puzzleTableObject.transform.localScale = new Vector3 (1, 1, 1);
		puzzleTableObject.transform.localPosition = new Vector3 (0,0,0);
		foreach(string tag in PuzzleTable.instance.puzzleTag){
			GameObject targetPrefab = Resources.Load ("Target/Target_" + tag) as GameObject;
			GameObject targetObject = Instantiate (targetPrefab) as GameObject;
			targetGrid.AddChild (targetObject.transform);
			targetObject.transform.localScale = new Vector3 (1,1,1);
		}
	}
		
	//パズル完成時に呼ばれる
	void CompletePuzzleEvent (string itemTag) {
		string id = itemTag.Remove (0, 7);

		FenceManager.instance.ShowFence ();
		GameObject getIdleDialogPrefab = Resources.Load ("Dialog/GetIdleDialog_" + id) as GameObject;
		GameObject getIdleDialogObject = Instantiate (getIdleDialogPrefab) as GameObject;
		getIdleDialogObject.transform.parent = GameObject.Find ("UI Root").transform;
		getIdleDialogObject.transform.localScale = new Vector3 (1, 1, 1);
		GetIdleDialogManager.instance.IdleID = Convert.ToInt32 (id);
	}

	//アイドルゲットダイアログを閉じた時に呼ばれる
	void ClosedDialogEvent (string dialogTag) {
		FenceManager.instance.HideFence ();
		UpdateGameEvent ();
		targetGrid.Reposition ();
	}

	//ゲームを更新する
	void UpdateGameEvent () {
		mUpdateCount++;
		if (mUpdateCount < targetGrid.GetChildList ().Count) {
			return;
		}
		mRemainingTapCount--;
		remainingTapCountLabel.text = "残りタップ" + mRemainingTapCount + "回";
		if (mRemainingTapCount <= 0) {
			//ゲーム終了
			FenceManager.instance.ShowFence ();
			GameObject finishPuzzleDialogPrefab = Resources.Load ("Dialog/FinishPuzzleDialog") as GameObject;
			GameObject finishPuzzleDialogObject = Instantiate (finishPuzzleDialogPrefab) as GameObject;
			finishPuzzleDialogObject.transform.parent = GameObject.Find ("UI Root").transform;
			finishPuzzleDialogObject.transform.localScale = new Vector3 (1, 1, 1);
			return;
		}
		mUpdateCount = 0;
	}

	//パズルを終了
	void FinishPuzzleEvent(){
		PlayerDataKeeper.instance.SaveData ();
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}
}
