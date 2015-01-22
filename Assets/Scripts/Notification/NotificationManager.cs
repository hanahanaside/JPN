using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour {

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
			Pause ();
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
	}

	private void Pause () {
		LocalNotification notification = new LocalNotification ();
		notification.applicationIconBadgeNumber = 1;
		notification.alertBody = "title";
		notification.fireDate = System.DateTime.Now.AddSeconds (5);
		NotificationServices.ScheduleLocalNotification (notification);

//		//最初のアイドルがサボった時の通知をスケジューリング
//		if (PrefsManager.instance.FirstIdolSleepNotificationON) {
//			MyLocalNotification myLocalNotification = new MyLocalNotification ();
//			myLocalNotification.ScheduleFirstIdolFallsSleep ();
//		}
//
//		//最後のアイドルがサボった時の通知をスケジューリング
//		if (PrefsManager.instance.LastIdolSleepNotificationON) {
//			MyLocalNotification myLocalNotification = new MyLocalNotification ();
//			myLocalNotification.ScheduleLastIdolFallsSleep ();
//		}
	}
}
