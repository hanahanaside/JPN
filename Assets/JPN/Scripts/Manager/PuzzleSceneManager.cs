using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	public GameObject puzzleTablePrefab;
	public Transform uiRoot;
	public Transform puzzleTableParent;
	public UILabel remainingTapCountLabel;
	public UIGrid targetGrid;

	private int mRemainingTapCount = 10;
	private int mContinueCount;
	private GameObject mPuzzleTableObject;
	private List<string> mGetItemTagList;

	void OnEnable () {
		Referee.UpdateGameEvent += UpdateGameEvent;
		Target.UpdateGameEvent += UpdateGameEvent;
		Target.CompleteTargetEvent += CompleteTargetEvent;
		GetIdleDialogManager.ClosedEvent += UpdateGameEvent;
		ContinueDialogManager.FinishPuzzleEvent += FinishPuzzleEvent;
		ContinueDialogManager.BuyTapCountEvent += BuyTapCountEvent;
		FinishPuzzleDialogManager.BackToStageEvent += BackToStageEvent;
		FinishPuzzleDialogManager.RetryEvent += RetryEvent;
		PuzzleTable.FinishedAnswerCheckEvent += FinishedAnswerCheckEvent;
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
		PuzzleTable.FinishedAnswerCheckEvent -= FinishedAnswerCheckEvent;
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
		mGetItemTagList.Add (targetTag);
		if (targetTag == "ticket") {
			return;
		}
		string id = targetTag.Remove (0, 5);
		FenceManager.instance.ShowFence ();
		GetIdleDialogManager.instance.Show (Convert.ToInt32 (id));
		//パズルクリアカウントを更新
		int[] clearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		clearedPuzzleCountArray [ScoutStageManager.SelectedAreaId - 1]++;
		PrefsManager.instance.ClearedPuzzleCountArray = clearedPuzzleCountArray;
		//	CharacterVoiceManager.instance.PlayVoice (Convert.ToInt32 (id) - 1);
		StartCoroutine ("PlayVoiceCoroutine", Convert.ToInt32 (id) - 1);
	}

	private IEnumerator PlayVoiceCoroutine (int id) {
		yield return new WaitForSeconds (0.5f);
		CharacterVoiceManager.instance.PlayVoice (id);
	}
		
	//ゲームを更新する
	void UpdateGameEvent () {
		mRemainingTapCount--;
		if (mRemainingTapCount > 0) {
			return;
		}
		if (mContinueCount >= 3) {
			FinishedAnswerCheckEvent ();
		} else if (targetGrid.GetChildList ().Count == 0) {
			FinishedAnswerCheckEvent ();
		} else {
			FenceManager.instance.ShowFence ();
			ContinueDialogManager.instance.Show ();
		}
	}

	//パズルを終了する
	void FinishPuzzleEvent () {
		FenceManager.instance.HideFence ();
		ContinueDialogManager.instance.Dismiss ();
		//答え合わせ
		FenceManager.instance.ShowTransparentFence ();
		PuzzleTable puzzleTable = puzzleTableParent.GetComponentInChildren<PuzzleTable> ();
		StartCoroutine (puzzleTable.AnswerCheck ());	
	}

	//答え合わせ終了時に呼ばれる
	void FinishedAnswerCheckEvent () {
		FenceManager.instance.HideTransparentFence ();
		FenceManager.instance.ShowFence ();
		FinishPuzzleDialogManager.instance.Show ();
	}

	//リトライ
	void RetryEvent (int coinCount) {
		FinishPuzzleDialogManager.instance.Dismiss ();
		FenceManager.instance.HideFence ();
		System.Collections.Generic.List<Transform> childList = targetGrid.GetChildList ();
		foreach (Transform childTransform in childList) {
			Destroy (childTransform.gameObject);
		}
		Destroy (mPuzzleTableObject);
		mRemainingTapCount = 10;
		mContinueCount = 0;
		CreatePuzzleTable ();
	}

	//タップを購入する
	void BuyTapCountEvent () {
		mRemainingTapCount += 5;
		mContinueCount++;
		FenceManager.instance.HideFence ();
		ContinueDialogManager.instance.Dismiss ();
	}
						
	//ステージにもどる
	void BackToStageEvent () {
		FlagBackButtonClicked = true;
		LoadLevelName.instance.loadLevelName = "Main";
		Invoke ("TransitionToMain", 1f);
	}

	private void TransitionToMain () {
		Application.LoadLevel ("Loading");
	}

	//獲得したアイテムのタグリストを返す
	public List<string> GetItemTagList {
		get {
			return mGetItemTagList;
		}
	}

	//パズルテーブルを作る
	private void CreatePuzzleTable () {
		#if UNITY_EDITOR
		if (ScoutStageManager.SelectedAreaId == 0) {
			ScoutStageManager.SelectedAreaId = 1;
		}
		#endif
		GameObject puzzleTablePrefab = Resources.Load ("PuzzleTable/PuzzleTable_" + ScoutStageManager.SelectedAreaId) as GameObject;
		mPuzzleTableObject = Instantiate (puzzleTablePrefab)as GameObject;
		mPuzzleTableObject.transform.parent = puzzleTableParent.transform;
		mPuzzleTableObject.transform.localPosition = new Vector3 (0, 0, 0);
		mPuzzleTableObject.transform.localScale = new Vector3 (1, 1, 1);
		PuzzleTable puzzleTable = mPuzzleTableObject.GetComponent<PuzzleTable> ();
		puzzleTable.CreateTable (ScoutStageManager.SelectedAreaId);
		mGetItemTagList = new List<string> ();
	}
}
