using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MyLocalNotification {

	#if UNITY_ANDROID
	private enum RequestCode : int {
		FIRST_TEAM,
		LAST_TEAM
	}
	#endif

	public MyLocalNotification () {

	}

	//最初のアイドルがサボった通知をスケジューリングする処理
	public void ScheduleFirstIdolFallsSleep () {
		if (CheckLiving ()) {
			return;
		}
		//1番小さいモノをスケジューリングの時間にセットする
		double addSeconds = GetFirstIdolSleepSeconds ();

		//全員がサボっている場合は0なので何もしない
		if (addSeconds <= 0) {
			MyLog.LogDebug ("全員サボっている");
			return;
		}

		#if UNITY_IPHONE
		ScheduleLocalNotification ("一組サボりがでました、カツを入れましょう", addSeconds);
		#endif

		#if UNITY_ANDROID
		ScheduleLocalNotification ("一組サボりがでました、カツを入れましょう", addSeconds, RequestCode.FIRST_TEAM);
		#endif

		MyLog.LogDebug ("最初のアイドルがサボる通知まで　" + addSeconds);
	}

	//最後のアイドルがサボった通知をスケジューリングする処理
	public void ScheduleLastIdolFallsSleep () {
		if (CheckLiving ()) {
			return;
		}

		//1番大きいモノをスケジューリングの時間にセットする
		double addSeconds = GetLastIdolSleepSeconds ();

		//全員がサボっている場合は0なので何もしない
		if (addSeconds <= 0) {
			MyLog.LogDebug ("全員サボっている");
			return;
		}

		#if UNITY_IPHONE
		ScheduleLocalNotification ("全組がサボりました、カツを入れましょう", addSeconds);
		#endif

		#if UNITY_ANDROID
		ScheduleLocalNotification ("全組がサボりました、カツを入れましょう", addSeconds, RequestCode.LAST_TEAM);
		#endif

		MyLog.LogDebug ("最後のアイドルがサボる通知まで　" + addSeconds);
	}

	//最初のアイドルがサボるまでの時間を取得
	private double GetFirstIdolSleepSeconds () {
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		double addSeconds = 0;
		foreach (StageManager stageManager in stageManagerList) {
			Stage stage = stageManager.Stage;
			if (stage.FlagConstruction == Stage.IN_CONSTRUCTION) {
				continue;
			}
			if (stageManager.GetState == StageManager.State.Sleep) {
				continue;
			}
			if (stageManager.UntilTime < addSeconds || addSeconds <= 0) {
				addSeconds = stageManager.UntilTime;
			}
		}
		return addSeconds;
	}

	//最後のアイドルがサボるまでの時間を取得
	private double GetLastIdolSleepSeconds () {
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		double addSeconds = 0;
		foreach (StageManager stageManager in stageManagerList) {
			Stage stage = stageManager.Stage;
			if (stage.FlagConstruction == Stage.IN_CONSTRUCTION) {
				continue;
			}
			if (stageManager.GetState == StageManager.State.Sleep) {
				continue;
			}
			if (stageManager.UntilTime > addSeconds) {
				addSeconds = stageManager.UntilTime;
			}
		}
		return addSeconds;
	}

	//ライブ中だったらtrueを返す
	private bool CheckLiving () {
		List<StageManager> stageManagerList = StageGridManager.instance.StageManagerList;
		StageManager hokkaidoStageManager = stageManagerList [0];
		if (hokkaidoStageManager.GetState == StageManager.State.Live) {
			return true;
		}
		return false;
	}

	#if UNITY_IPHONE
	//通知をスケジューリングする(iOS)
	private void ScheduleLocalNotification (string message, double addSeconds) {
		LocalNotification localNotification = new LocalNotification ();
		localNotification.applicationIconBadgeNumber = 1;
		localNotification.fireDate = DateTime.Now.AddSeconds (addSeconds);
		localNotification.alertBody = message;
		NotificationServices.ScheduleLocalNotification (localNotification);
	}
	#endif

	#if UNITY_ANDROID
	//通知をスケジューリングする(Android)
	private void ScheduleLocalNotification (string message, double addSeconds, RequestCode requestCode) {
		long secondsFromNow = (long)addSeconds;
		string title = "アイプロ";
		string subTitle = message;
		string tickerText = message;
		string extraData = "extraData";
		EtceteraAndroid.scheduleNotification (secondsFromNow, title, subTitle, tickerText, extraData, (int)requestCode);
	}
	#endif
}
