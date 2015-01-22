using UnityEngine;
using System.Collections;


public class TestController : TestParent {
	public GameObject adObject;
	public UILabel label;
	private int mIndex;


	enum Days{Sun = 5,Mon,Tue};

	void Start(){
		string dateString = "2014/10/12";
		System.DateTime dt = System.DateTime.Parse (dateString);
		Debug.Log ("aaa " +dt);
	}

	void OnApplicationPause(bool pauseStatus){
		Debug.Log ("ssss");
		if (pauseStatus) {
			LocalNotification notification = new LocalNotification ();
			notification.applicationIconBadgeNumber = 1;
			notification.alertBody = "title";
			notification.fireDate = System.DateTime.Now.AddSeconds (5);
			NotificationServices.ScheduleLocalNotification (notification);
		} else {
			LocalNotification clearBadgeNotification = new LocalNotification();
			clearBadgeNotification.applicationIconBadgeNumber = -1;
			NotificationServices.PresentLocalNotificationNow(clearBadgeNotification);
			NotificationServices.CancelAllLocalNotifications ();
			NotificationServices.ClearRemoteNotifications();
			NotificationServices.ClearLocalNotifications();
		}
	}

	public void ButtonClicked(){
		Debug.Log ("click");
//		SuruPassAdBanner.instance.Hide ();
	}

	public void FinishedTypeWriter(){
		Debug.Log ("finished");
	}
}
