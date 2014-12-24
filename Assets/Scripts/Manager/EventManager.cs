using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class EventManager : MonoSingleton<EventManager> {

	public GameObject sleepButtonObject;
	public GameObject liveButtonObject;
	public GameObject LostButtonObject;
	public GameObject TransferButtonObject;
	public GameObject newsButtonObject;
	public GameObject eventPanelObject;
	public UILabel messageLabel;

	private int mSleepStageCount;
	private LostIdleEvent mLostIdleEvent;

	public void Init () {
		mLostIdleEvent = Resources.Load ("Data/LostIdleEvent") as LostIdleEvent;
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
			TransferButtonObject.SetActive (true);
			break;
		case 2:
			//ニュース
			newsButtonObject.SetActive (true);
			break;
		}

	}

	public void GenerateLostIdle () {
		//迷子のアイドルがいれば生成してアラートを表示
		if (mLostIdleEvent.lostIdleCount >= 1) {
			LostButtonObject.SetActive (true);
			StageGridManager.instance.GenerateLostIdle (mLostIdleEvent.lostIdleID, mLostIdleEvent.lostIdleCount);
		}

	}

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
			TransferButtonObject.SetActive (false);
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
	}

	public void TransferButtonClicked () {
		StringBuilder sb = new StringBuilder ();
		sb.Append ("新潟の子かわいいね！\n");
		sb.Append ("3人を2000コインでうちに\n");
		sb.Append ("移籍させてくれないか？");
		ShowEventPanel (sb.ToString ());
	}

	public void NewsButtonClicked () {
		StringBuilder sb = new StringBuilder ();
		sb.Append ("おめでとう！\n");
		sb.Append ("賞を受賞したよ！\n");
		sb.Append ("2000コインゲットだ！");
		ShowEventPanel (sb.ToString ());
		newsButtonObject.SetActive (false);
	}

	public void YesButtonClicked () {
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
	}

	public void NoButtonClicked () {

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
