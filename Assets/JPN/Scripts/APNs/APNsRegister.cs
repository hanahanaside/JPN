using UnityEngine;
using MiniJSON;
using System.Net;
using System.IO;
using System.Collections;

public class APNsRegister : MonoSingleton<APNsRegister> {
	private const string TT5_URL = "http://push.tt5.us/api/receive";
	public int projectId;

	#if UNITY_IPHONE
	
	void OnEnable () {
		EtceteraManager.remoteRegistrationSucceededEvent += remoteRegistrationSucceeded;
		EtceteraManager.remoteRegistrationFailedEvent += remoteRegistrationFailed;
	}

	void OnDisable () {
		EtceteraManager.remoteRegistrationSucceededEvent -= remoteRegistrationSucceeded;
		EtceteraManager.remoteRegistrationFailedEvent -= remoteRegistrationFailed;
	}

	void Awake () {
		if (!PrefsManager.instance.APNsRegisted) {
			DontDestroyOnLoad (gameObject);
		}
	}

	void remoteRegistrationFailed (string error) {
		Debug.Log ("remoteRegistrationFailed : " + error);
	}

	void remoteRegistrationSucceeded (string deviceToken) {
		Debug.Log ("remoteRegistrationSucceeded : " + deviceToken);
		
		string osVersion = SystemInfo.operatingSystem.Replace ("iPhone OS ", "");
		string platform = iPhone.generation.ToString ();
		Debug.Log ("osVersion = " + osVersion);
		Debug.Log ("platform = " + platform);
		Debug.Log ("projectId = " + projectId);
		WWWForm form = new WWWForm ();
		form.AddField ("v", 0);
		form.AddField ("pid", projectId);
		form.AddField ("os_version", osVersion);
		form.AddField ("platform", platform);
		form.AddField ("device_token", deviceToken);
		StartCoroutine (SendDeviceToken (form));
	}

	public void RegisterForRemoteNotifcations () {
		EtceteraBinding.registerForRemoteNotifcations (P31RemoteNotificationType.Alert | P31RemoteNotificationType.Badge | P31RemoteNotificationType.Sound);
	}

	private IEnumerator SendDeviceToken (WWWForm wwwForm) {
		Debug.Log ("SendDeviceToken");
		WWW www = new WWW (TT5_URL, wwwForm);
		yield return www;
		
		// check for errors
		if (www.error == null) {
			Debug.Log ("WWW Ok!: " + www.text);
			PrefsManager.instance.APNsRegisted = true;
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}
		Destroy (gameObject);
	}
	
	#endif
}
