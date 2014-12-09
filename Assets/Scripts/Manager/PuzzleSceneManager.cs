using UnityEngine;
using System.Collections;
using System;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	public Transform uiRoot;
	public Transform puzzleTableParent;
	public GameObject finishPuzzleDialogPrefab;
	public UILabel remainingTapCountLabel;

	private int mRemainingTapCount = 10;

	public GameObject puzzleTablePrefab;

	void OnEnable () {
		Referee.UpdateGameEvent += UpdateGameEvent;
		Target.UpdateGameEvent += UpdateGameEvent;
		Puzzle.OpenedPuzzleEvent += OpenedPuzzleEvent;
		Target.CompleteTargetEvent += CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent += UpdateGameEvent;
	}

	void OnDisable () {
		Referee.UpdateGameEvent -= UpdateGameEvent;
		Target.UpdateGameEvent -= UpdateGameEvent;
		Puzzle.OpenedPuzzleEvent -= OpenedPuzzleEvent;
		Target.CompleteTargetEvent -= CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent -= UpdateGameEvent;
	}

	void Start () {
		PlayerDataKeeper.instance.Init ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
		GameObject puzzleTableObject = Instantiate (puzzleTablePrefab) as GameObject;
		puzzleTableObject.transform.parent = puzzleTableParent.transform;
		puzzleTableObject.transform.localPosition = new Vector3 (0, 0, 0);
		puzzleTableObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	void Update () {
		remainingTapCountLabel.text = "残りタップ" + mRemainingTapCount + "回";
	}

	//パズル完成アニメーション終了時に呼ばれる
	void CompleteTargetEvent (string targetTag) {
		string id = targetTag.Remove (0, 7);
		FenceManager.instance.ShowFence ();
		GameObject getIdleDialogPrefab = Resources.Load ("Dialog/GetIdleDialog_" + id) as GameObject;
		GameObject getIdleDialogObject = Instantiate (getIdleDialogPrefab) as GameObject;
		getIdleDialogObject.transform.parent = uiRoot.transform;
		getIdleDialogObject.transform.localScale = new Vector3 (1, 1, 1);
		GetIdleDialogManager getIdleManager = getIdleDialogObject.GetComponentInChildren<GetIdleDialogManager> ();
		getIdleManager.Show (Convert.ToInt32 (id));
	}

	//パズルオープン時に呼ばれる
	void OpenedPuzzleEvent (string tag) {
		mRemainingTapCount--;
	}

	void UpdateGameEvent () {
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
