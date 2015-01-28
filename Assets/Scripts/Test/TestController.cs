using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	void Start(){
		EtceteraAndroid.cancelAllNotifications();
	}
		
	void OnApplicationPause (bool pauseStatus) {
		if(pauseStatus){
			Debug.Log ("pause");
			EtceteraAndroid.scheduleNotification(20,"title","subTitle","tickerText","",0);
		}else {
			Debug.Log ("resume");
			EtceteraAndroid.cancelAllNotifications();
		}
	}
}
