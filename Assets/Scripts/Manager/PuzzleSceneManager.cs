using UnityEngine;
using System.Collections;
using System;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	public GameObject puzzleTablePrefab;
	public GameObject continueDialogObject;
	public GameObject finishPuzzleDialogObject;
	public GameObject getIdleDialogObject;
	public Transform uiRoot;
	public Transform puzzleTableParent;
	public UILabel remainingTapCountLabel;
	public UIGrid targetGrid;

	private int mRemainingTapCount = 10;
	private GameObject mPuzzleTableObject;


	void OnEnable () {
		Referee.UpdateGameEvent += UpdateGameEvent;
		Target.UpdateGameEvent += UpdateGameEvent;
		Target.CompleteTargetEvent += CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent += UpdateGameEvent;
		ContinueDialogManager.FinishPuzzleEvent += FinishPuzzleEvent;
		ContinueDialogManager.BuyTapCountEvent += BuyTapCountEvent;
		FinishPuzzleDialogManager.BackToStageEvent += BackToStageEvent;
		FinishPuzzleDialogManager.RetryEvent += RetryEvent;
	}

	void OnDisable () {
		Referee.UpdateGameEvent -= UpdateGameEvent;
		Target.UpdateGameEvent -= UpdateGameEvent;
		Target.CompleteTargetEvent -= CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent -= UpdateGameEvent;
		ContinueDialogManager.FinishPuzzleEvent -= FinishPuzzleEvent;
		ContinueDialogManager.BuyTapCountEvent -= BuyTapCountEvent;
		FinishPuzzleDialogManager.BackToStageEvent -= BackToStageEvent;
		FinishPuzzleDialogManager.RetryEvent -= RetryEvent;
	}

	void Start () {
		Debug.Log ("level " + ScoutStageManager.SelectedAreaId);
		PlayerDataKeeper.instance.Init ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
		CreatePuzzleTable ();
	}

	void Update () {
		remainingTapCountLabel.text = "残りタップ" + mRemainingTapCount + "回";
	}

	//パズル完成アニメーション終了時に呼ばれる
	void CompleteTargetEvent (string targetTag) {
		targetGrid.repositionNow = true;
		string id = targetTag.Remove (0, 5);
		FenceManager.instance.ShowFence ();
		getIdleDialogObject.SetActive (true);
		GetIdleDialogManager getIdleManager = getIdleDialogObject.GetComponentInChildren<GetIdleDialogManager> ();
		getIdleManager.Show (Convert.ToInt32 (id));
		//パズルクリアカウントを更新
		int[] clearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		clearedPuzzleCountArray [ScoutStageManager.SelectedAreaId - 1]++;
		PrefsManager.instance.ClearedPuzzleCountArray = clearedPuzzleCountArray;
	}
		
	//ゲームを更新する
	void UpdateGameEvent () {
		mRemainingTapCount--;
		if (mRemainingTapCount <= 0) {
			FenceManager.instance.ShowFence ();
			continueDialogObject.SetActive (true);
		}
	}

	//パズルを終了する
	void FinishPuzzleEvent () {
		continueDialogObject.SetActive (false);
		finishPuzzleDialogObject.SetActive (true);
		FinishPuzzleDialogManager manager = finishPuzzleDialogObject.GetComponentInChildren<FinishPuzzleDialogManager> ();
		manager.Show ();
	}

	//リトライ
	void RetryEvent (int coinCount) {
		finishPuzzleDialogObject.SetActive (false);
		FenceManager.instance.HideFence ();
		System.Collections.Generic.List<Transform> childList = targetGrid.GetChildList ();
		foreach (Transform childTransform in childList) {
			Destroy (childTransform.gameObject);
		}
		Destroy (mPuzzleTableObject);
		mRemainingTapCount = 10;
		CreatePuzzleTable ();
	}

	//タップを購入する
	void BuyTapCountEvent () {
		mRemainingTapCount += 5;
		FenceManager.instance.HideFence ();
		continueDialogObject.SetActive (false);
	}
						
	//ステージにもどる
	void BackToStageEvent () {
		PlayerDataKeeper.instance.SaveData ();
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}

	//パズルテーブルを作る
	private void CreatePuzzleTable () {
		GameObject puzzleTablePrefab = Resources.Load ("PuzzleTable/PuzzleTable_" + ScoutStageManager.SelectedAreaId) as GameObject;
		mPuzzleTableObject = Instantiate (puzzleTablePrefab)as GameObject;
		mPuzzleTableObject.transform.parent = puzzleTableParent.transform;
		mPuzzleTableObject.transform.localPosition = new Vector3 (0, 0, 0);
		mPuzzleTableObject.transform.localScale = new Vector3 (1, 1, 1);
		PuzzleTable puzzleTable = mPuzzleTableObject.GetComponent<PuzzleTable> ();
		puzzleTable.CreateTable ();
	}
}
