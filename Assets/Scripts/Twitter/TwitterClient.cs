using UnityEngine;
using System.Text;
using System.Collections;

public class TwitterClient : MonoSingleton<TwitterClient> {

	void OnEnable () {
		TwitterManager.tweetSheetCompletedEvent += tweetSheetCompletedEvent;
	}
	
	void OnDisable () {
		TwitterManager.tweetSheetCompletedEvent -= tweetSheetCompletedEvent;
	}

	void Start () {
		DontDestroyOnLoad (gameObject);
	}
		
	void tweetSheetCompletedEvent (bool didSucceed) {
		Debug.Log ("tweetSheetCompletedEvent " + didSucceed);
		if (didSucceed) {
		
		}
	}

	public void Tweet (string text) {
		Debug.Log ("Tweet");

		#if UNITY_IPHONE
		TwitterBinding.showTweetComposer(text);
		#endif
	
#if UNITY_ANDROID
		SocialConnector.Share(text);
#endif
	}

}
