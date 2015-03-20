using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainSceneManager : MonoSingleton<MainSceneManager> {

	void OnEnable () {
		CoinController.OnClickedEvent += OnCoinClickedEvent;
	}

	void OnDisable () {
		CoinController.OnClickedEvent -= OnCoinClickedEvent;
	}

	void Start () {
		MyLog.LogDebug ("start main scene manager");
		PlayerDataKeeper.instance.Init ();
		EventManager.instance.Init ();
		StageGridManager.instance.CreateStageGrid ();
		MoveStagePanelManager.instance.CreateMoveStageGrid ();
		//パズル終わりであればスカウト画面から再開
		if (ScoutStageManager.FlagScouting) {
			StageGridManager.instance.MoveToStage (0);
			ScoutStageManager.instance.PlayMoveInPlaneAnimation ();
			//新規でエリア解放可能なモノがあればそれを表示
			string newUnlockAreaName = CheckNewUnlockAreaExist ();
			if (!string.IsNullOrEmpty (newUnlockAreaName)) {
				EventManager.instance.okButtonClickedEvent += EventOKButtonClicked;
				EventManager.instance.ShowNatsumoto (newUnlockAreaName + "が購入可能になりました");
			}
			#if !UNITY_EDITOR
			AdManager.instance.ShowInterstitialAd ();
			#endif
		} else {
			StageGridManager.instance.MoveToStage (1);
		}
			
		Resume ();
		EventManager.instance.GenerateLostIdle ();

		AdManager.instance.ShowBannerAd ();
	}

	void Update () {
		#if UNITY_ANDROID
		if (Input.GetKey (KeyCode.Escape)) {
			CheckQuitDialog.instance.Show();
		}
		#endif
	}

	void OnApplicationPause (bool pauseStatus) {
		if (pauseStatus) {
			MyLog.LogDebug ("pause");
			#if !UNITY_EDITOR
			//プレイヤーデータをセーブ
			PlayerDataKeeper.instance.SaveData ();
			if(!LiveManager.instance.IsLive){
			NotificationManager.instance.ScheduleLocalNotification ();
			}
			#endif
		} else {
			MyLog.LogDebug ("resume");
			#if !UNITY_EDITOR
			Resume();
			//時間関係の処理の指令を出す
			StageGridManager.instance.Resume ();
			#endif
		}
	}

	void EventOKButtonClicked () {
		EventManager.instance.okButtonClickedEvent -= EventOKButtonClicked;
		AreaPanelManager.instance.ShowAreaPanel ();
	}

	//起動時に呼ばれる
	private void Resume () {
		//中断中に稼いだコインを取得
		double addCoin = GeneratedCoinCalculator.CalcWhileSleeping ();
		Debug.Log ("add coin = " + addCoin);
		PlayerDataKeeper.instance.IncreaseCoinCount (addCoin);
		//表示するダイアログの確認をする
		CheckDialogs (addCoin);

		//ライブの途中であれば再開
		float remainingLiveTimeSeconds = GetRemainingLiveTimeSeconds ();
		LiveData liveData = PrefsManager.instance.Read<LiveData> (PrefsManager.Kies.LiveData);
		if (remainingLiveTimeSeconds > 0) {
			LiveManager.instance.ContinueLive (remainingLiveTimeSeconds);
		} else if (LiveManager.instance.IsLive) {
			LiveManager.instance.FinishLive ();
		} else if (liveData.time > 0) {
			//ライブデータをリセット
			liveData = new LiveData ();
			PrefsManager.instance.WriteData<LiveData> (liveData, PrefsManager.Kies.LiveData);
			//全てのステージのアップデート履歴を更新
			List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
			foreach (StageManager stageManager in stageManagerList) {
				stageManager.FinishLive ();
			}
			CoinGenerator.instance.FinishLive ();
			SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
		} else {
			SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
		}
	}

	//起動時に表示するダイアログのうち表示できるモノを1つ表示する
	private void CheckDialogs (double addCoin) {
		//ResumeCountをインクリメント
		int resumeCount = PrefsManager.instance.ResumeCount;
		resumeCount++;
		PrefsManager.instance.ResumeCount = resumeCount;

		//addCoinが0を超えていたらコインのダイアログを出す
		if (addCoin >= 100) {
			FenceManager.instance.ShowFence ();
			SleepTimeCoinDialogManager.instance.Show (addCoin);
			return;
		}
		if (resumeCount < 10) {
			return;
		}
		if (resumeCount % 5 != 0) {
			return;
		}
		//レビュー済みの場合はオススメアプリダイアログを出す
		if (PrefsManager.instance.IsReviewed) {
			RecommendAppDialog.instance.Show ();
		} else {
			ReviewDialog.instance.Show ();
		}
	}
		
	//新規でアンロック可能なエリアの名前を返す
	private string CheckNewUnlockAreaExist () {
		int[] clearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		for (int i = 0; i < clearedPuzzleCountArray.Length; i++) {
			int clearCount = clearedPuzzleCountArray [i];
			if (clearCount != (int)AreaPanelManager.AreaState.NotYetPurchased) {
				continue;
			}
			//未購入のエリアがある場合は購入可能かをチェックする
			Entity_Area entityArea = Resources.Load ("Data/Area") as Entity_Area;
			Entity_Area.Param param = entityArea.param [i];
			//既にアナウンス済みの場合はbreak
			int announcedUnlockAreaCount = PrefsManager.instance.AnnouncedUnlockAreaCount;
			if (announcedUnlockAreaCount >= param.area_id) {
				break;
			}
			int totalIdleCount = 0;
			StageDao dao = DaoFactory.CreateStageDao ();
			List<StageData> stageList = dao.SelectAll ();
			foreach (StageData stage in stageList) {
				totalIdleCount += stage.IdolCount;
			}
			if (totalIdleCount > param.minimum_amount) {
				PrefsManager.instance.AnnouncedUnlockAreaCount = param.area_id;
				return param.area_name;
			}
		}
		return "";
	}
	
		
	//コインタップ時の処理
	void OnCoinClickedEvent (string tag) {
		switch (tag) {
		case "coin_1":
			PlayerDataKeeper.instance.IncreaseCoinCount (1.0);
			break;
		case "coin_5":
			PlayerDataKeeper.instance.IncreaseCoinCount (5.0);
			break;
		case "coin_25":
			PlayerDataKeeper.instance.IncreaseCoinCount (25.0);
			break;
		case "coin_100":
			PlayerDataKeeper.instance.IncreaseCoinCount (100.0);
			break;
		case "coin_1000":
			PlayerDataKeeper.instance.IncreaseCoinCount (1000.0);
			break;
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
	}

	public void OnScoutButtonClicked () {
		MoveStagePanelManager.instance.HideMoveStagePanel ();
		StageGridManager.instance.MoveToStage (0);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnManagementButtonClicked () {
		ManagementPanelManager.instance.ShowManagementPanel ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnMoveStageButtonClicked () {
		MoveStagePanelManager.instance.ShowMoveStagePanel ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnMenuButtonClicked () {
		FenceManager.instance.ShowFence ();
		MenuPanelManager.instance.ShowMenuPanel ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnTicketClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		BuyTicketDialog.instance.Show ();
	}

	private float GetRemainingLiveTimeSeconds () {
		LiveData liveData = PrefsManager.instance.Read<LiveData> (PrefsManager.Kies.LiveData);
		//ライブが始まっていなければ0を返す
		if (liveData.time <= 0) {
			return 0;
		}
		DateTime dtNow = DateTime.Now;
		DateTime dtLive = DateTime.Parse (liveData.startDate);
		TimeSpan timeSpan = dtNow - dtLive;
		float remainingLiveTimeSeconds = (float)(liveData.time - timeSpan.TotalSeconds);
		return remainingLiveTimeSeconds;
	}
}
