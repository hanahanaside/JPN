using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationManager : MonoSingleton<NotificationManager> {

	void OnEnable () {
		#if UNITY_IPHONE
		EtceteraManager.localNotificationWasReceivedAtLaunchEvent += localNotificationWasReceivedAtLaunchEvent;
		EtceteraManager.remoteNotificationReceivedAtLaunchEvent += remoteNotificationReceivedAtLaunch;
		#endif
	}

	void OnDisable () {
		#if UNITY_IPHONE
		EtceteraManager.localNotificationWasReceivedAtLaunchEvent -= localNotificationWasReceivedAtLaunchEvent;
		EtceteraManager.remoteNotificationReceivedAtLaunchEvent -= remoteNotificationReceivedAtLaunch;
		#endif
	}

	void Awake () {
		#if !UNITY_EDITOR
		DontDestroyOnLoad (gameObject);
		Resume();
		#endif
	}

	void OnApplicationPause (bool pauseStatus) {
		#if !UNITY_EDITOR
		if(pauseStatus){
			
		}else {
			Resume ();
		}
		#endif
	}

	void localNotificationWasReceivedAtLaunchEvent (IDictionary notification) {
		Debug.Log ("localNotificationWasReceivedAtLaunchEvent");
		Prime31.Utils.logObject (notification);
		Resume ();
	}

	void remoteNotificationReceivedAtLaunch (IDictionary notification) {
		Debug.Log ("remoteNotificationReceivedAtLaunch");
		Prime31.Utils.logObject (notification);
		Resume ();
	}

	public void ScheduleLocalNotification(){

		//通知を完全にOFFにしていたら何もしない
		if (!PrefsManager.instance.FirstIdolSleepNotificationON && !PrefsManager.instance.LastIdolSleepNotificationON) {
			return;
		}

		StageDao stageDao = DaoFactory.CreateStageDao ();
		List<StageData> stageDataList = stageDao.SelectAll ();

		//最初のアイドルがサボった時の通知をスケジューリング
		if (PrefsManager.instance.FirstIdolSleepNotificationON) {
			MyLocalNotification myLocalNotification = new MyLocalNotification ();
			myLocalNotification.ScheduleFirstIdolFallsSleep (stageDataList);
		}

		//最後のアイドルがサボった時の通知をスケジューリング
		if (PrefsManager.instance.LastIdolSleepNotificationON) {
			MyLocalNotification myLocalNotification = new MyLocalNotification ();
			myLocalNotification.ScheduleLastIdolFallsSleep (stageDataList);
		}
	}

	private void Resume () {
		#if UNITY_IPHONE
		LocalNotification clearBadgeNotification = new LocalNotification ();
		clearBadgeNotification.applicationIconBadgeNumber = -1;
		//ローカル通知の2回分をクリアする
		for (int i = 0; i < 2; i++) {
			MyLog.LogDebug ("バッジをクリア");
			NotificationServices.PresentLocalNotificationNow (clearBadgeNotification);
		}
		NotificationServices.CancelAllLocalNotifications ();
		NotificationServices.ClearRemoteNotifications ();
		NotificationServices.ClearLocalNotifications ();
		#endif

		#if UNITY_ANDROID
		EtceteraAndroid.cancelAllNotifications();
		#endif
	}
}
