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
		MyLog.LogDebug ("start");
		PlayerDataKeeper.instance.Init ();
		StageGridManager.instance.CreateStageGrid ();
		EventManager.instance.Init ();
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
			SuruPassInterstitial.instance.Show ();
			#endif
		} else {
			StageGridManager.instance.MoveToStage (1);
			double addCoin = CalcSleepTimeCoin ();
			if (addCoin >= 1) {
				FenceManager.instance.ShowFence ();
				SleepTimeCoinDialogManager.instance.Show (addCoin);
			}
		}
			
		Resume ();

		EventManager.instance.GenerateLostIdle ();
		#if !UNITY_EDITOR
		SuruPassAdBanner.instance.Show ();
		#endif
	}

	void Update () {
		#if UNITY_ANDROID
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}
		#endif
	}

	void OnApplicationPause (bool pauseStatus) {
		if (pauseStatus) {
			MyLog.LogDebug ("pause");
			#if !UNITY_EDITOR
			//プレイヤーデータをセーブ
			PlayerDataKeeper.instance.SaveData ();
			#endif
		} else {
			MyLog.LogDebug ("resume");
			#if !UNITY_EDITOR
			Resume();
			//時間関係の処理の指令を出す
			StageGridManager.instance.Resume ();
			//レビューダイアログを表示
			int resumeCount = PrefsManager.instance.ResumeCount;
			resumeCount++;
			PrefsManager.instance.ResumeCount = resumeCount;
			if (resumeCount < 10) {
			return;
			}
			if (PrefsManager.instance.IsReviewed) {
			return;
			}
			if (resumeCount % 5 == 0) {
			ReviewDialog.instance.Show ();
			}
			#endif
		}
	}

	void EventOKButtonClicked () {
		EventManager.instance.okButtonClickedEvent -= EventOKButtonClicked;
		AreaPanelManager.instance.ShowAreaPanel ();
	}

	private void Resume () {
		//中断中に稼いだコインを取得
		double addCoin = CalcSleepTimeCoin ();

		//ライブの途中であれば再開
		float remainingLiveTimeSeconds = GetRemainingLiveTimeSeconds ();
		if (remainingLiveTimeSeconds > 0) {
			LiveManager.instance.ContinueLive (remainingLiveTimeSeconds);
		} else {
			SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
		}
		PlayerDataKeeper.instance.IncreaseCoinCount (addCoin);
	}

	//中断中に稼いだコインを計算して返す
	private double CalcSleepTimeCoin () {
		DateTime dtNow = DateTime.Now;
		DateTime dtExit = DateTime.Parse (PlayerDataKeeper.instance.ExitDate);
		TimeSpan ts = dtNow - dtExit;
		Debug.Log ("ts " + ts.TotalSeconds);
		double addCoin = (PlayerDataKeeper.instance.SavedGenerateCoinPower / 60.0) * ts.TotalSeconds;
		float remainingLiveTimeSeconds = GetRemainingLiveTimeSeconds ();
		if (remainingLiveTimeSeconds > 0) {
			addCoin = addCoin * 2;
		}
		Debug.Log ("addCoin " + addCoin);
		return addCoin;
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
			List<Stage> stageList = dao.SelectAll ();
			foreach (Stage stage in stageList) {
				totalIdleCount += stage.IdleCount;
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
		BuyTicketDialog.instance.Show ();
	}

	private float GetRemainingLiveTimeSeconds () {
		LiveData liveData = PrefsManager.instance.Read<LiveData> (PrefsManager.Kies.LiveData);
		DateTime dtNow = DateTime.Now;
		DateTime dtLive = DateTime.Parse (liveData.startDate);
		TimeSpan timeSpan = dtNow - dtLive;
		float remainingLiveTimeSeconds = (float)(liveData.time - timeSpan.TotalSeconds);
		return remainingLiveTimeSeconds;
	}
}
