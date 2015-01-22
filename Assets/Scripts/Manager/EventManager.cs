using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;

public class EventManager : MonoSingleton<EventManager> {

	public event Action okButtonClickedEvent;

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
	public GameObject buttonParentObject;
	public GameObject lostIdolOKButtonObject;
	public UILabel messageLabel;
	public UILabel coinLabel;
	public UILabel lostIdolRewardLabel;
	public UILabel lostIdolCountLabel;

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
		//イベントの条件が整えば、イベントを発生させる
		//迷子イベント
		DateTime dtNow = DateTime.Now;
		DateTime dtLastUpdate = DateTime.Parse (mLostIdleEvent.LastUpdateDate);
		TimeSpan timeSpan = dtNow - dtLastUpdate;
//		if(timeSpan.TotalHours >= 12){
//			RaiseLostIdleEvent ();
//		}

		if (timeSpan.TotalSeconds >= 12) {
			RaiseLostIdleEvent ();
		}


		//トレードイベント
		dtLastUpdate = DateTime.Parse (mTradeIdleEvent.LastUpdateDate);
		timeSpan = dtNow - dtLastUpdate;
		if (timeSpan.TotalHours >= 12) {
			OccurTradeIdleEvent ();
		}

		//ニュースイベント
		dtLastUpdate = DateTime.Parse (mNewsEvent.LastUpdateDate);
		timeSpan = dtNow - dtLastUpdate;
		if (timeSpan.TotalHours >= 12) {
			OccurNewsEvent ();
		}
			
		if (mTradeIdleEvent.occurring) {
			TradeButtonObject.SetActive (true);
		}
		if (mNewsEvent.occurring) {
			newsButtonObject.SetActive (true);
		}
		buttonParentObject.SetActive (false);
	}

	public void GenerateLostIdle () {
		//迷子のアイドルがいれば生成してアラートを表示
		if (mLostIdleEvent.occurring) {
			LostButtonObject.SetActive (true);
			lostIdolCountLabel.text = "×" + (mLostIdleEvent.lostIdleCount - mLostIdleEvent.foundIdleCount);
			StageGridManager.instance.GenerateLostIdle (mLostIdleEvent.lostIdleID, mLostIdleEvent.lostIdleCount - mLostIdleEvent.foundIdleCount);
		}
	}

	public void ShowNatsumoto (string message) {
		okButtonObject.SetActive (true);
		ShowEventPanel (message);
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
		if (stage.FlagConstruction == Stage.IN_CONSTRUCTION) {
			Debug.Log ("工事中のアイドルなので迷子を中止");
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
		mLostIdleEvent.occurring = true;
		//迷子の報酬を算出
		GenerateCoinPowerDao generateCoinPowerDao = DaoFactory.CreateGenerateCoinPowerDao ();
		double generateCoinPower = generateCoinPowerDao.SelectById (stage.Id, stage.IdleCount);
		mLostIdleEvent.reward = (int)(generateCoinPower * 100 * mLostIdleEvent.lostIdleCount);
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
		//トレードの金額を算出
		GenerateCoinPowerDao generateCoinPowerDao = DaoFactory.CreateGenerateCoinPowerDao ();
		double generateCoinPower = generateCoinPowerDao.SelectById (stage.Id, stage.IdleCount);
		mTradeIdleEvent.reward = (int)(generateCoinPower * 150 * mTradeIdleEvent.idleCount);
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
		if (mSleepStageCount >= 10) {
			liveButtonObject.SetActive (true);
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
		lostIdolCountLabel.text = "×" + (mLostIdleEvent.lostIdleCount - mLostIdleEvent.foundIdleCount);
		Debug.Log ("find count " + mLostIdleEvent.foundIdleCount);
		if (mLostIdleEvent.foundIdleCount >= mLostIdleEvent.lostIdleCount) {
			lostIdolRewardLabel.text = "" + mLostIdleEvent.reward;
			StringBuilder sb = new StringBuilder ();
			sb.Append ("すごい！\n");
			sb.Append ("すべて見つけたんだね！\n");
			sb.Append (mLostIdleEvent.reward + "コインゲット!!");
			ShowEventPanel (sb.ToString ());
			lostIdolOKButtonObject.SetActive (true);
			StageDao dao = DaoFactory.CreateStageDao ();
			List<Stage> stageList = dao.SelectAll ();
			Stage stage = stageList [mLostIdleEvent.lostIdleID - 1];
			stage.IdleCount += mLostIdleEvent.lostIdleCount;
			dao.UpdateRecord (stage);
			StageGridManager.instance.GenerateIdle (mLostIdleEvent.lostIdleID, mLostIdleEvent.lostIdleCount);
			mLostIdleEvent.occurring = false;
			mLostIdleEvent.LastUpdateDate = DateTime.Now.ToString ();
			LostButtonObject.SetActive (false);
		}
		PrefsManager.instance.WriteData<LostIdleEvent> (mLostIdleEvent, PrefsManager.Kies.LostIdleEvent);
	}

	void CompleteDismissEvent () { 
		FenceManager.instance.HideFence ();
		eventPanelObject.transform.localPosition = new Vector3 (0, 0, 0);
		eventPanelObject.SetActive (false);
	}

	public void FinishedTypeWriter () { 
		buttonParentObject.SetActive (true);
	}

	public void SleepButtonClicked () {
		StageGridManager.instance.MoveToSleepStage ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void LiveButtonClicked () {
		StageGridManager.instance.MoveToStage (1);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		FenceManager.instance.ShowFence ();
		ConfirmLiveDialog.instance.Show ();
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
		okButtonObject.SetActive (true);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.LostIdol);
	}

	public void TransferButtonClicked () {
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (mTradeIdleEvent.idleID);
		StringBuilder sb = new StringBuilder ();
		sb.Append (stage.AreaName + "の子はすごく人気だね！！\n");
		sb.Append (mTradeIdleEvent.idleCount + "人を" + mTradeIdleEvent.reward + "コインでうちの事務所に移籍させてくれないかな？\n");
		sb.Append ("（現在" + stage.IdleCount + "人）");
		ShowEventPanel (sb.ToString ());
		coinLabel.text = "" + mTradeIdleEvent.reward;
		yesButtonObject.SetActive (true);
		noButtonObject.SetActive (true);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.TradeIdol);
	}

	public void NewsButtonClicked () {
		StringBuilder sb = new StringBuilder ();
		sb.Append (mNewsEvent.message + "\n");
		sb.Append (mNewsEvent.reward + "チケットゲットだ！");
		ShowEventPanel (sb.ToString ());
		mNewsEvent.occurring = false;
		mNewsEvent.LastUpdateDate = DateTime.Now.ToString ();
		newsButtonObject.SetActive (false);
		newsOKButtonObject.SetActive (true);
		PrefsManager.instance.WriteData<NewsEvent> (mNewsEvent, PrefsManager.Kies.NewsEvent);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.News);
	}

	//移籍の時のみ発動
	public void YesButtonClicked () {
		HideButtons ();
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
		HideButtons ();
		mTradeIdleEvent.occurring = false;
		mTradeIdleEvent.LastUpdateDate = DateTime.Now.ToString ();
		TradeButtonObject.SetActive (false);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		PrefsManager.instance.WriteData<TradeIdleEvent> (mTradeIdleEvent, PrefsManager.Kies.TradeIdleEvent);
	}

	//迷子を全員発見
	public void LostIdolOKButtonClicked () {
		HideButtons ();
		PlayerDataKeeper.instance.IncreaseCoinCount (mLostIdleEvent.reward);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
	}

	public void OKButtonClicked () {
		HideButtons ();
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if (okButtonClickedEvent != null) {
			okButtonClickedEvent ();
		}
	}

	public void NewsOKButtonClicked () {
		HideButtons ();
		PlayerDataKeeper.instance.IncreaseTicketCount (1);
		iTweenEvent.GetEvent (eventPanelObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.TradeIdol);
	}

	private void ShowEventPanel (string text) {
		FenceManager.instance.ShowFence ();
		eventPanelObject.SetActive (true);
		iTweenEvent.GetEvent (eventPanelObject, "ShowEvent").Play ();
		messageLabel.text = text;
		messageLabel.gameObject.GetComponent<TypewriterEffect> ().ResetToBeginning ();
	}

	private void StartScaleEvent (GameObject buttonObject) {
		iTweenEvent.GetEvent (buttonObject, "ScaleEvent").Play ();
	}

	private void StopScaleEvent (GameObject buttonObject) {
		iTweenEvent.GetEvent (buttonObject, "ScaleEvent").Stop ();
		buttonObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	private void HideButtons () {
		yesButtonObject.SetActive (false);
		noButtonObject.SetActive (false);
		okButtonObject.SetActive (false);
		lostIdolOKButtonObject.SetActive (false);
		newsOKButtonObject.SetActive (false);
		buttonParentObject.SetActive (false);
	}
}
