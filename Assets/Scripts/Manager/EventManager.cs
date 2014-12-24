﻿using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class EventManager : MonoSingleton<EventManager> {

	public GameObject sleepButtonObject;
	public GameObject liveButtonObject;
	public GameObject LostButtonObject;
	public GameObject TradeButtonObject;
	public GameObject newsButtonObject;
	public GameObject eventPanelObject;
	public GameObject okButtonObject;
	public GameObject yesButtonObject;
	public GameObject noButtonObject;
	public UILabel messageLabel;

	private int mSleepStageCount;
	private LostIdleEvent mLostIdleEvent;
	private TradeIdleEvent mTradeIdleEvent;

	public void Init () {
		//全てのイベント情報を取得
		mLostIdleEvent = Resources.Load ("Data/LostIdleEvent") as LostIdleEvent;
		mTradeIdleEvent = Resources.Load ("Data/TradeIdleEvent") as TradeIdleEvent;

		//	int[] eventIdArray = { -1, -1, -1, -1, -1, -1, -1, 0, 1, 2 };
		int[] eventIdArray = { 0, 1, 2 };
		int rand = UnityEngine.Random.Range (0, eventIdArray.Length);
		int eventId = eventIdArray [rand];
		Debug.Log ("event " + eventId);
		switch (eventId) {
		case 0:
			//迷子
			RaiseLostIdleEvent ();
			break;
		case 1:
			//移籍
			OccurTradeIdleEvent ();
			break;
		case 2:
			//ニュース
			newsButtonObject.SetActive (true);
			break;
		}
		if (mTradeIdleEvent.occurring) {
			TradeButtonObject.SetActive (true);
		}
	}

	public void GenerateLostIdle () {
		//迷子のアイドルがいれば生成してアラートを表示
		if (mLostIdleEvent.lostIdleCount >= 1) {
			LostButtonObject.SetActive (true);
			StageGridManager.instance.GenerateLostIdle (mLostIdleEvent.lostIdleID, mLostIdleEvent.lostIdleCount);
		}
	}

	//迷子イベントを発生させる
	private void RaiseLostIdleEvent () {
		//迷子のアイドルが0人だったらイベント開始
		if (mLostIdleEvent.lostIdleCount <= 0) {
			StageDao dao = DaoFactory.CreateStageDao ();
			List<Stage> stageList = dao.SelectAll ();
			int rand = UnityEngine.Random.Range (0, stageList.Count);
			Stage stage = stageList [rand];
			int count = UnityEngine.Random.Range (0, stage.IdleCount);
			stage.IdleCount -= count;
			dao.UpdateRecord (stage);
			mLostIdleEvent.lostIdleID = stage.Id;
			mLostIdleEvent.lostIdleCount = count;
			mLostIdleEvent.foundIdleCount = 0;
			mLostIdleEvent.reward = stage.Id * count * 10;
			Debug.Log ("id " + stage.Id);
			Debug.Log ("count " + count);
		}
	}

	//トレードイベントを発生させる
	private void OccurTradeIdleEvent () {
		//発生中だったら何もしない
		if (mTradeIdleEvent.occurring) {
			Debug.Log ("い");
			return;
		}
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		int rand = UnityEngine.Random.Range (0, stageList.Count);
		Stage stage = stageList [rand];
		//アイドルの数が1人以下だったら何もしない
		if (stage.IdleCount <= 1) {
			Debug.Log ("あ");
			return;
		}
		mTradeIdleEvent.idleID = stage.Id;
		mTradeIdleEvent.idleCount = UnityEngine.Random.Range (1, stage.IdleCount);
		mTradeIdleEvent.reward = stage.Id * mTradeIdleEvent.idleCount * 100;
		mTradeIdleEvent.occurring = true;
		Debug.Log ("発生");
	}

	void OnEnable () {
		StageManager.SleepEvent += SleepEvent;
		StageManager.WakeupEvent += WakeupEvent;
		Idle.FoundEvent += FoundIdleEvent;
	}

	void OnDisable () {
		StageManager.SleepEvent -= SleepEvent;
		StageManager.WakeupEvent -= WakeupEvent;
		Idle.FoundEvent -= FoundIdleEvent;
	}

	void SleepEvent () {
		mSleepStageCount++;
		if (!sleepButtonObject.activeSelf) {
			sleepButtonObject.SetActive (true);
			StartScaleEvent (sleepButtonObject);
		}
		if (mSleepStageCount >= StageGridManager.instance.StageCount) {
			liveButtonObject.SetActive (true);
		}
	}

	void WakeupEvent () {
		mSleepStageCount--;
		if (mSleepStageCount <= 0) {
			StopScaleEvent (sleepButtonObject);
			sleepButtonObject.SetActive (false);
		}
		if (liveButtonObject.activeSelf) {
			liveButtonObject.SetActive (false);
		}
	}

	void FoundIdleEvent (Character character) {
		mLostIdleEvent.foundIdleCount++;
		Debug.Log ("find count " + mLostIdleEvent.foundIdleCount);
		if (mLostIdleEvent.foundIdleCount == mLostIdleEvent.lostIdleCount) {
			PlayerDataKeeper.instance.IncreaseCoinCount (mLostIdleEvent.reward);
			StringBuilder sb = new StringBuilder ();
			sb.Append ("すごい！\n");
			sb.Append ("すべて見つけたんだね！\n");
			sb.Append (mLostIdleEvent.reward + "コインゲット!!");
			ShowEventPanel (sb.ToString ());
			TradeButtonObject.SetActive (false);
			StageDao dao = DaoFactory.CreateStageDao ();
			List<Stage> stageList = dao.SelectAll ();
			Stage stage = stageList [mLostIdleEvent.lostIdleID - 1];
			stage.IdleCount += mLostIdleEvent.lostIdleCount;
			dao.UpdateRecord (stage);
			StageGridManager.instance.GenerateIdle ();
			mLostIdleEvent.lostIdleCount = 0;
			LostButtonObject.SetActive (false);
		}
	}

	void CompleteDismissEvent () { 
		FenceManager.instance.HideFence ();
		eventPanelObject.transform.localPosition = new Vector3 (0, 0, 0);
		eventPanelObject.SetActive (false);
	}

	public void SleepButtonClicked () {
		StageGridManager.instance.MoveToSleepStage ();
	}

	public void LiveButtonClicked () {
		StageGridManager.instance.MoveToStage (1);
	}

	public void LostButtonClicked () {
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (mLostIdleEvent.lostIdleID);
		StringBuilder sb = new StringBuilder ();
		sb.Append ("大変だ！\n");
		sb.Append (stage.AreaName + "のアイドルが " + mLostIdleEvent.lostIdleCount + "人迷子になったぞ！\n");
		sb.Append ("他の都道府県にまぎれこんでいるから、見つけたらタップしよう！\n");
		sb.Append ("すべて見つけたらボーナスだ！");
		ShowEventPanel (sb.ToString ());
		yesButtonObject.SetActive (false);
		noButtonObject.SetActive (false);
		okButtonObject.SetActive (true);
	}

	public void TransferButtonClicked () {
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (mTradeIdleEvent.idleID);
		StringBuilder sb = new StringBuilder ();
		sb.Append (stage.AreaName + "はすごく人気だね！！\n");
		sb.Append (mTradeIdleEvent.idleCount + "人を" + mTradeIdleEvent.reward + "コインでうちの事務所に移籍させてくれないかな？\n");
		ShowEventPanel (sb.ToString ());
		yesButtonObject.SetActive (true);
		noButtonObject.SetActive (true);
		okButtonObject.SetActive (false);
	}

	public void NewsButtonClicked () {
		StringBuilder sb = new StringBuilder ();
		sb.Append ("おめでとう！\n");
		sb.Append ("賞を受賞したよ！\n");
		sb.Append ("2000コインゲットだ！");
		ShowEventPanel (sb.ToString ());
		newsButtonObject.SetActive (false);
		yesButtonObject.SetActive (false);
		noButtonObject.SetActive (false);
		okButtonObject.SetActive (true);
	}

	//移籍の時のみ発動
	public void YesButtonClicked () {
		PlayerDataKeeper.instance.IncreaseCoinCount (mTradeIdleEvent.reward);
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (mTradeIdleEvent.idleID);
		stage.IdleCount -= mTradeIdleEvent.idleCount;
		dao.UpdateRecord (stage);
		mTradeIdleEvent.occurring = false;
		TradeButtonObject.SetActive (false);
		StageGridManager.instance.RemoveIdle (mTradeIdleEvent.idleID, mTradeIdleEvent.idleCount);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
	}

	//移籍の時のみ発動
	public void NoButtonClicked () {
		mTradeIdleEvent.occurring = false;
		TradeButtonObject.SetActive (false);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
	}

	public void OKButtonClicked () {
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
	}

	private void ShowEventPanel (string text) {
		FenceManager.instance.ShowFence ();
		eventPanelObject.SetActive (true);
		iTweenEvent.GetEvent (eventPanelObject, "ShowEvent").Play ();
		messageLabel.gameObject.GetComponent<TypewriterEffect> ().ResetToBeginning ();
		messageLabel.text = text;
	}

	private void StartScaleEvent (GameObject buttonObject) {
		iTweenEvent.GetEvent (buttonObject, "ScaleEvent").Play ();
	}

	private void StopScaleEvent (GameObject buttonObject) {
		iTweenEvent.GetEvent (buttonObject, "ScaleEvent").Stop ();
		buttonObject.transform.localScale = new Vector3 (1, 1, 1);
	}
}
