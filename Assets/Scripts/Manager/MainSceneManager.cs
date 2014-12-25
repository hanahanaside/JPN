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
		EventManager.instance.Init ();
		PlayerDataKeeper.instance.Init ();
		StageGridManager.instance.CreateStageGrid ();
		MoveStagePanelManager.instance.CreateMoveStageGrid ();
		if (ScoutStageManager.FlagScouting) {
			StageGridManager.instance.MoveToStage (0);
			ScoutStageManager.instance.PlayMoveInPlaneAnimation ();
		} else {
			StageGridManager.instance.MoveToStage (1);
		}

			CalcSleepTimeCoin ();


		EventManager.instance.GenerateLostIdle ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
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
			//イベント情報をセーブ
			EventManager.instance.SaveEvent ();
			#endif
		} else {
			MyLog.LogDebug ("resume");
			#if !UNITY_EDITOR
			CalcSleepTimeCoin ();
			//時間関係の処理の指令を出す
			StageGridManager.instance.Resume ();
			#endif
		}
	}

	private void CalcSleepTimeCoin(){
		DateTime dtNow = DateTime.Now;
		DateTime dtExit = DateTime.Parse (PlayerDataKeeper.instance.ExitDate);
		TimeSpan ts = dtNow - dtExit;
		Debug.Log ("ts " + ts.TotalSeconds);
		double addCoin = (PlayerDataKeeper.instance.SavedGenerateCoinPower / 60.0) * ts.TotalSeconds;
		Debug.Log ("addCoin " +addCoin);
		PlayerDataKeeper.instance.IncreaseCoinCount (addCoin);
		FenceManager.instance.ShowFence ();
		SleepTimeCoinDialogManager.instance.Show (addCoin);
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
}
