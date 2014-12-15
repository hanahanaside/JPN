using UnityEngine;
using System.Collections;
using System;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	public GameObject getIdleDialogPrefab;
	public Transform uiRoot;
	public Transform puzzleTableParent;
	public GameObject finishPuzzleDialogPrefab;
	public UILabel remainingTapCountLabel;

	private int mRemainingTapCount = 10;

	public GameObject puzzleTablePrefab;

	void OnEnable () {
		Referee.UpdateGameEvent += UpdateGameEvent;
		Target.UpdateGameEvent += UpdateGameEvent;
		Target.CompleteTargetEvent += CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent += UpdateGameEvent;
		FinishPuzzleDialogManager.FinishPuzzleEvent += FinishPuzzleEvent;
	}

	void OnDisable () {
		Referee.UpdateGameEvent -= UpdateGameEvent;
		Target.UpdateGameEvent -= UpdateGameEvent;
		Target.CompleteTargetEvent -= CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent -= UpdateGameEvent;
		FinishPuzzleDialogManager.FinishPuzzleEvent -= FinishPuzzleEvent;
	}

	void Start () {
		Debug.Log ("level " +ScoutStageManager.SelectedAreaId);
		PlayerDataKeeper.instance.Init ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
		GameObject puzzleTablePrefab = Resources.Load ("PuzzleTable/PuzzleTable_" + ScoutStageManager.SelectedAreaId) as GameObject;
		GameObject puzzleTableObject = Instantiate (puzzleTablePrefab)as GameObject;
		puzzleTableObject.transform.parent = puzzleTableParent.transform;
		puzzleTableObject.transform.localPosition = new Vector3 (0, 0, 0);
		puzzleTableObject.transform.localScale = new Vector3 (1, 1, 1);
		PuzzleTable puzzleTable = puzzleTableObject.GetComponent<PuzzleTable> ();
		puzzleTable.CreateTable (1);
	}

	void Update () {
		remainingTapCountLabel.text = "残りタップ" + mRemainingTapCount + "回";
	}

	//パズル完成アニメーション終了時に呼ばれる
	void CompleteTargetEvent (string targetTag) {
		string id = targetTag.Remove (0, 5);
		FenceManager.instance.ShowFence ();
		GameObject getIdleDialogObject = Instantiate (getIdleDialogPrefab) as GameObject;
		getIdleDialogObject.transform.parent = uiRoot.transform;
		getIdleDialogObject.transform.localScale = new Vector3 (1, 1, 1);
		GetIdleDialogManager getIdleManager = getIdleDialogObject.GetComponentInChildren<GetIdleDialogManager> ();
		getIdleManager.Show (Convert.ToInt32 (id));
	}
		
	//ゲームを更新する
	void UpdateGameEvent () {
		mRemainingTapCount--;
		if (mRemainingTapCount <= 0) {
			FenceManager.instance.ShowFence ();
			GameObject finishPuzzleDialogObject = Instantiate (finishPuzzleDialogPrefab) as GameObject;
			finishPuzzleDialogObject.transform.parent = uiRoot.transform;
			finishPuzzleDialogObject.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

						
	//パズルを終了
	void FinishPuzzleEvent () {
		PlayerDataKeeper.instance.SaveData ();
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}
}
