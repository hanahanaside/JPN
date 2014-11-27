using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		MoveStagePanelManager.instance.CreateMoveStageGrid ();
		if (ScoutStageManager.FlagScouting) {
			StageGridManager.instance.MoveToStage (0);
			ScoutStageManager.instance.PlayMoveInPlaneAnimation ();
		} else {
			StageGridManager.instance.MoveToStage (1);
			EventManager.instance.Init ();
		}
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
	}

	public void OnApplicationPause (bool pauseStatus) {
		if (pauseStatus) {
			MyLog.LogDebug ("pause");
			//プレイヤーデータをセーブ
			PlayerDataKeeper.instance.SaveData ();
		} else {
			MyLog.LogDebug ("resume");
			//時間関係の処理の指令を出す
		}
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
		MenuPanelManager.instance.ShowMenuPanel ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}
}
