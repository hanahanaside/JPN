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
	public void ScheduleFirstIdolFallsSleep (List<StageData> stageDataList) {
		//1番小さいモノをスケジューリングの時間にセットする
		double addSeconds = GetFirstIdolSleepSeconds (stageDataList);

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
	public void ScheduleLastIdolFallsSleep (List<StageData> stageDataList) {

		//1番大きいモノをスケジューリングの時間にセットする
		double addSeconds = GetLastIdolSleepSeconds (stageDataList);

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
	private double GetFirstIdolSleepSeconds (List<StageData> stageDataList) {
		double addSeconds = 0;
		//サボるまでの時間を順に比較していって、一番小さいステージを採用する
		foreach (StageData stageData in stageDataList) {
			if (stageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
				continue;
			}
			//サボっているかをチェック
			if (CheckSleep (stageData)) {
				continue;
			}
			double untilSleepTimeSec = UntilSleepTimeSec(stageData);
			if(addSeconds <= 0){
				addSeconds = untilSleepTimeSec;
				continue;
			}
			if(untilSleepTimeSec < addSeconds){
				addSeconds = untilSleepTimeSec;
			}
		}
		return addSeconds;
	}

	//最後のアイドルがサボるまでの時間を取得
	private double GetLastIdolSleepSeconds (List<StageData> stageDataList) {
		double addSeconds = 0;
		//サボるまでの時間を順に比較していって、一番大きいステージを採用する
		foreach (StageData stageData in stageDataList) {
			if (stageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
				continue;
			}
			//サボっているかをチェック
			if (CheckSleep (stageData)) {
				continue;
			}
			double untilSleepTimeSec = UntilSleepTimeSec(stageData);
			if(addSeconds <= 0){
				addSeconds = untilSleepTimeSec;
				continue;
			}
			if(untilSleepTimeSec > addSeconds){
				addSeconds = untilSleepTimeSec;
			}
		}
		return addSeconds;
	}

	//サボっていたらtrueを返す
	private bool CheckSleep (StageData stageData) {
		DateTime dtNow = DateTime.Now;
		DateTime dtUpdatedDate = DateTime.Parse (stageData.UpdatedDate);
		UntilSleepTimeDao untilSleepTimeDao = DaoFactory.CreateUntilSleepTimeDao ();
		int untilSleepTimeMin = untilSleepTimeDao.SelectById (stageData.Id, stageData.IdolCount);
		double untilSleepTimeSec = untilSleepTimeMin * 60;
		TimeSpan ts = dtNow - dtUpdatedDate;
		if (ts.TotalSeconds > untilSleepTimeSec) {
			return true;
		}
		return false;
	}

	//サボるまでの時間を返す
	private double UntilSleepTimeSec(StageData stageData){
		DateTime dtNow = DateTime.Now;
		DateTime dtUpdatedDate = DateTime.Parse (stageData.UpdatedDate);
		UntilSleepTimeDao untilSleepTimeDao = DaoFactory.CreateUntilSleepTimeDao ();
		int untilSleepTimeMin = untilSleepTimeDao.SelectById (stageData.Id, stageData.IdolCount);
		double untilSleepTimeSec = untilSleepTimeMin * 60;
		TimeSpan ts = dtNow - dtUpdatedDate;
		return untilSleepTimeSec - ts.TotalSeconds;
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
