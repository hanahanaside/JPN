using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoSingleton<UnityAdsManager> {

	#if UNITY_IPHONE
	private const string GAME_ID = "131622360";
	#endif

	#if UNITY_ANDROID
	private const string GAME_ID = "131622999";
	#endif

	void ResultCallback (ShowResult result) {
		ContinueDialogManager.instance.FinishedUnityAds (result);
	}

	public override void OnInitialize () {
		DontDestroyOnLoad (gameObject);
		#if !UNITY_EDITOR
		Advertisement.Initialize (GAME_ID);
		#endif
	}

	public void ShowAd () {
		ShowOptions showOptions = new ShowOptions ();
		showOptions.resultCallback += ResultCallback;
		showOptions.pause = true;
		Advertisement.Show (null, showOptions);
	}

	public bool IsReady () {
		return Advertisement.isReady ();
	}
}
