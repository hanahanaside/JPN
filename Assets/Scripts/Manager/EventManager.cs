using UnityEngine;
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
	public UILabel coinLabel;

	private int mSleepStageCount;
	public LostIdleEvent lostIdleEvent;
	public TradeIdleEvent tradeIdleEvent;
	public NewsEvent newsEvent;

	public void Init () {
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
			OccurNewsEvent ();
			break;
		}
		if (tradeIdleEvent.occurring) {
			TradeButtonObject.SetActive (true);
		}
		if (newsEvent.occurring) {
			newsButtonObject.SetActive (true);
		}
	}

	public void GenerateLostIdle () {
		//迷子のアイドルがいれば生成してアラートを表示
		if (lostIdleEvent.lostIdleCount >= 1) {
			LostButtonObject.SetActive (true);
			StageGridManager.instance.GenerateLostIdle (lostIdleEvent.lostIdleID, lostIdleEvent.lostIdleCount);
		}
	}

	//迷子イベントを発生させる
	private void RaiseLostIdleEvent () {
		//迷子のアイドルが0人だったらイベント開始
		if (lostIdleEvent.lostIdleCount <= 0) {
			StageDao dao = DaoFactory.CreateStageDao ();
			List<Stage> stageList = dao.SelectAll ();
			int rand = UnityEngine.Random.Range (0, stageList.Count);
			Stage stage = stageList [rand];
			if (stage.IdleCount <= 2) {
				Debug.Log ("アイドルが2人以下なので迷子を中止");
				return;
			}
			int count = UnityEngine.Random.Range (1, stage.IdleCount);
			stage.IdleCount -= count;
			dao.UpdateRecord (stage);
			lostIdleEvent.lostIdleID = stage.Id;
			lostIdleEvent.lostIdleCount = count;
			lostIdleEvent.foundIdleCount = 0;
			lostIdleEvent.reward = stage.Id * count * 10;
			Debug.Log ("id " + stage.Id);
			Debug.Log ("count " + count);
		}
	}

	//トレードイベントを発生させる
	private void OccurTradeIdleEvent () {
		//発生中だったら何もしない
		if (tradeIdleEvent.occurring) {
			TradeButtonObject.SetActive (true);
			return;
		}
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		int rand = UnityEngine.Random.Range (0, stageList.Count);
		Stage stage = stageList [rand];
		//アイドルの数が1人以下だったら何もしない
		if (stage.IdleCount <= 1) {
			return;
		}
		tradeIdleEvent.idleID = stage.Id;
		tradeIdleEvent.idleCount = UnityEngine.Random.Range (1, stage.IdleCount);
		tradeIdleEvent.reward = stage.Id * tradeIdleEvent.idleCount * 100;
		tradeIdleEvent.occurring = true;
		TradeButtonObject.SetActive (true);
	}

	//ニュースイベントを発生させる
	private void OccurNewsEvent () {
		//発生中だったら何もしない
		if (newsEvent.occurring) {
			newsButtonObject.SetActive (true);
			return;
		}
		Entity_News entityNews = Resources.Load ("Data/News") as Entity_News;
		int rand = UnityEngine.Random.Range (0, entityNews.param.Count);
		newsEvent.message = entityNews.param [rand].message;
		newsEvent.reward = entityNews.param [rand].reward;
		newsEvent.unit = entityNews.param [rand].unit;
		newsEvent.occurring = true;
		newsButtonObject.SetActive (true);
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
		lostIdleEvent.foundIdleCount++;
		Debug.Log ("find count " + lostIdleEvent.foundIdleCount);
		if (lostIdleEvent.foundIdleCount == lostIdleEvent.lostIdleCount) {
			PlayerDataKeeper.instance.IncreaseCoinCount (lostIdleEvent.reward);
			StringBuilder sb = new StringBuilder ();
			sb.Append ("すごい！\n");
			sb.Append ("すべて見つけたんだね！\n");
			sb.Append (lostIdleEvent.reward + "コインゲット!!");
			ShowEventPanel (sb.ToString ());
			TradeButtonObject.SetActive (false);
			StageDao dao = DaoFactory.CreateStageDao ();
			List<Stage> stageList = dao.SelectAll ();
			Stage stage = stageList [lostIdleEvent.lostIdleID - 1];
			stage.IdleCount += lostIdleEvent.lostIdleCount;
			dao.UpdateRecord (stage);
			StageGridManager.instance.GenerateIdle ();
			lostIdleEvent.lostIdleCount = 0;
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
		Stage stage = dao.SelectById (lostIdleEvent.lostIdleID);
		StringBuilder sb = new StringBuilder ();
		sb.Append ("大変だ！\n");
		sb.Append (stage.AreaName + "のアイドルが " + lostIdleEvent.lostIdleCount + "人迷子になったぞ！\n");
		sb.Append ("他の都道府県にまぎれこんでいるから、見つけたらタップしよう！\n");
		sb.Append ("すべて見つけたらボーナスだ！");
		ShowEventPanel (sb.ToString ());
		yesButtonObject.SetActive (false);
		noButtonObject.SetActive (false);
		okButtonObject.SetActive (true);
	}

	public void TransferButtonClicked () {
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (tradeIdleEvent.idleID);
		StringBuilder sb = new StringBuilder ();
		sb.Append (stage.AreaName + "はすごく人気だね！！\n");
		sb.Append (tradeIdleEvent.idleCount + "人を" + tradeIdleEvent.reward + "コインでうちの事務所に移籍させてくれないかな？\n");
		ShowEventPanel (sb.ToString ());
		coinLabel.text = "" + tradeIdleEvent.reward;
		yesButtonObject.SetActive (true);
		noButtonObject.SetActive (true);
		okButtonObject.SetActive (false);
	}

	public void NewsButtonClicked () {
		string unit = "";
		if (newsEvent.unit == "ticket") {
			PlayerDataKeeper.instance.IncreaseTicketCount (newsEvent.reward);
			unit = "チケット";
		} else {
			PlayerDataKeeper.instance.IncreaseCoinCount (newsEvent.reward);
			unit = "コイン";
		}
		StringBuilder sb = new StringBuilder ();
		sb.Append ("おめでとう！\n");
		sb.Append (newsEvent.message + "\n");
		sb.Append (newsEvent.reward + unit + "ゲットだ！");
		ShowEventPanel (sb.ToString ());
		newsEvent.occurring = false;
		newsButtonObject.SetActive (false);
		yesButtonObject.SetActive (false);
		noButtonObject.SetActive (false);
		okButtonObject.SetActive (true);
	}

	//移籍の時のみ発動
	public void YesButtonClicked () {
		PlayerDataKeeper.instance.IncreaseCoinCount (tradeIdleEvent.reward);
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (tradeIdleEvent.idleID);
		stage.IdleCount -= tradeIdleEvent.idleCount;
		dao.UpdateRecord (stage);
		tradeIdleEvent.occurring = false;
		TradeButtonObject.SetActive (false);
		StageGridManager.instance.RemoveIdle (tradeIdleEvent.idleID, tradeIdleEvent.idleCount);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
	}

	//移籍の時のみ発動
	public void NoButtonClicked () {
		tradeIdleEvent.occurring = false;
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
