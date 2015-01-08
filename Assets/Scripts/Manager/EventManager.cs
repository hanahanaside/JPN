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
	public GameObject newsOKButtonObject;
	public GameObject yesButtonObject;
	public GameObject noButtonObject;
	public UILabel messageLabel;
	public UILabel coinLabel;

	private int mSleepStageCount;
	private LostIdleEvent mLostIdleEvent;
	private TradeIdleEvent mTradeIdleEvent;
	private NewsEvent mNewsEvent;

	public void Init () {
		mLostIdleEvent = PrefsManager.instance.Read<LostIdleEvent> (PrefsManager.Kies.LostIdleEvent);
		mTradeIdleEvent = PrefsManager.instance.Read<TradeIdleEvent> (PrefsManager.Kies.TradeIdleEvent);
		mNewsEvent = PrefsManager.instance.Read<NewsEvent> (PrefsManager.Kies.NewsEvent);

		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		if (stageList.Count <= 5) {
			return;
		}
		//	int[] eventIdArray = { -1, -1, -1, -1, -1, -1, -1, 0, 1, 2 };
		int[] eventIdArray = { 0, 1, 2 };
		int rand = UnityEngine.Random.Range (0, eventIdArray.Length);
		int eventId = eventIdArray [rand];
		Debug.Log ("event " + eventId);
		//	eventId = 1;
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
		if (mTradeIdleEvent.occurring) {
			TradeButtonObject.SetActive (true);
		}
		if (mNewsEvent.occurring) {
			newsButtonObject.SetActive (true);
		}
	}

	public void GenerateLostIdle () {
		//迷子のアイドルがいれば生成してアラートを表示
		if (mLostIdleEvent.occurring) {
			LostButtonObject.SetActive (true);
			StageGridManager.instance.GenerateLostIdle (mLostIdleEvent.lostIdleID, mLostIdleEvent.lostIdleCount - mLostIdleEvent.foundIdleCount);
		}
	}
		
	//迷子イベントを発生させる
	private void RaiseLostIdleEvent () {
		MyLog.LogDebug ("迷子イベント");
		//迷子のアイドルが0人だったらイベント開始
		if (mLostIdleEvent.occurring) {
			Debug.Log ("発生中");
			return;
		}

		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		int rand = UnityEngine.Random.Range (0, stageList.Count);
		Stage stage = stageList [rand];
		if (stage.IdleCount <= 1) {
			Debug.Log ("アイドルが1人以下なので迷子を中止");
			return;
		}
		int count = UnityEngine.Random.Range (1, 16);
		if (count >= stage.IdleCount) {
			return;
		}
		stage.IdleCount -= count;
		dao.UpdateRecord (stage);
		mLostIdleEvent.lostIdleID = stage.Id;
		mLostIdleEvent.lostIdleCount = count;
		mLostIdleEvent.foundIdleCount = 0;
		mLostIdleEvent.reward = stage.Id * count * 10;
		mLostIdleEvent.occurring = true;
		PrefsManager.instance.WriteData<LostIdleEvent> (mLostIdleEvent, PrefsManager.Kies.LostIdleEvent);
		Debug.Log ("id " + stage.Id);
		Debug.Log ("count " + count);
		MyLog.LogDebug ("迷子イベント開始");
	}

	//トレードイベントを発生させる
	private void OccurTradeIdleEvent () {
		Debug.Log ("トレードイベント");
		//発生中だったら何もしない
		if (mTradeIdleEvent.occurring) {
			TradeButtonObject.SetActive (true);
			Debug.Log ("発生中");
			return;
		}
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		int rand = UnityEngine.Random.Range (0, stageList.Count);
		Stage stage = stageList [rand];
		//アイドルの数が1人以下だったら何もしない
		if (stage.IdleCount <= 1) {
			Debug.Log ("アイドルが1人以下なのでトレードを中止");
			return;
		}
		mTradeIdleEvent.idleID = stage.Id;
		mTradeIdleEvent.idleCount = UnityEngine.Random.Range (1, 6);
		if (mTradeIdleEvent.idleCount >= stage.IdleCount) {
			return;
		}
		mTradeIdleEvent.reward = stage.Id * mTradeIdleEvent.idleCount * 100;
		mTradeIdleEvent.occurring = true;
		TradeButtonObject.SetActive (true);
		PrefsManager.instance.WriteData<TradeIdleEvent> (mTradeIdleEvent, PrefsManager.Kies.TradeIdleEvent);
		Debug.Log ("トレードイベント開始");
	}

	//ニュースイベントを発生させる
	private void OccurNewsEvent () {
		Debug.Log ("ニュースイベント");
		//発生中だったら何もしない
		if (mNewsEvent.occurring) {
			newsButtonObject.SetActive (true);
			Debug.Log ("発生中");
			return;
		}
		Entity_News entityNews = Resources.Load ("Data/News") as Entity_News;
		int rand = UnityEngine.Random.Range (0, entityNews.param.Count);
		mNewsEvent.message = entityNews.param [rand].message;
		mNewsEvent.reward = entityNews.param [rand].reward;
		mNewsEvent.unit = entityNews.param [rand].unit;
		mNewsEvent.occurring = true;
		newsButtonObject.SetActive (true);
		PrefsManager.instance.WriteData<NewsEvent> (mNewsEvent, PrefsManager.Kies.NewsEvent);
		Debug.Log ("ニュースイベント開始");
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
		if (mLostIdleEvent.foundIdleCount >= mLostIdleEvent.lostIdleCount) {
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
			StageGridManager.instance.GenerateIdle (mLostIdleEvent.lostIdleID);
			mLostIdleEvent.occurring = false;
			LostButtonObject.SetActive (false);
		}
		PrefsManager.instance.WriteData<LostIdleEvent> (mLostIdleEvent, PrefsManager.Kies.LostIdleEvent);
	}

	void CompleteDismissEvent () { 
		FenceManager.instance.HideFence ();
		eventPanelObject.transform.localPosition = new Vector3 (0, 0, 0);
		eventPanelObject.SetActive (false);
	}

	public void SleepButtonClicked () {
		StageGridManager.instance.MoveToSleepStage ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void LiveButtonClicked () {
		StageGridManager.instance.MoveToStage (1);
		EntranceStageManager.instance.OnLiveButtonClicked ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void LostButtonClicked () {
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (mLostIdleEvent.lostIdleID);
		StringBuilder sb = new StringBuilder ();
		sb.Append ("大変だ！\n");
		sb.Append (stage.AreaName + "のアイドルが " + (mLostIdleEvent.lostIdleCount - mLostIdleEvent.foundIdleCount) + "人迷子になったぞ！\n");
		sb.Append ("他の都道府県にまぎれこんでいるから、見つけたらタップしよう！\n");
		sb.Append ("すべて見つけたらボーナスだ！");
		ShowEventPanel (sb.ToString ());
		yesButtonObject.SetActive (false);
		noButtonObject.SetActive (false);
		okButtonObject.SetActive (true);
		newsOKButtonObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.LostIdol);
	}

	public void TransferButtonClicked () {
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (mTradeIdleEvent.idleID);
		StringBuilder sb = new StringBuilder ();
		sb.Append (stage.AreaName + "の子はすごく人気だね！！\n");
		sb.Append (mTradeIdleEvent.idleCount + "人を" + mTradeIdleEvent.reward + "コインでうちの事務所に移籍させてくれないかな？\n");
		ShowEventPanel (sb.ToString ());
		coinLabel.text = "" + mTradeIdleEvent.reward;
		yesButtonObject.SetActive (true);
		noButtonObject.SetActive (true);
		okButtonObject.SetActive (false);
		newsOKButtonObject.SetActive (false);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.TradeIdol);
	}

	public void NewsButtonClicked () {
		StringBuilder sb = new StringBuilder ();
		sb.Append (mNewsEvent.message + "\n");
		sb.Append (mNewsEvent.reward + "チケットゲットだ！");
		ShowEventPanel (sb.ToString ());
		mNewsEvent.occurring = false;
		newsButtonObject.SetActive (false);
		yesButtonObject.SetActive (false);
		noButtonObject.SetActive (false);
		okButtonObject.SetActive (false);
		newsOKButtonObject.SetActive (true);
		PrefsManager.instance.WriteData<NewsEvent> (mNewsEvent, PrefsManager.Kies.NewsEvent);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.News);
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
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
		PrefsManager.instance.WriteData<TradeIdleEvent> (mTradeIdleEvent, PrefsManager.Kies.TradeIdleEvent);
	}

	//移籍の時のみ発動
	public void NoButtonClicked () {
		mTradeIdleEvent.occurring = false;
		TradeButtonObject.SetActive (false);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		PrefsManager.instance.WriteData<TradeIdleEvent> (mTradeIdleEvent, PrefsManager.Kies.TradeIdleEvent);
	}

	public void OKButtonClicked () {
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void NewsOKButtonClicked () {
		PlayerDataKeeper.instance.IncreaseTicketCount (1);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
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
