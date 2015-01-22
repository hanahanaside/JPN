using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoSingleton<UnityAdsManager> {

	private const string GAME_ID = "131622360";

	void ResultCallback (ShowResult result) {
		ContinueDialogManager.instance.FinishedUnityAds (result);
	}

	public override void OnInitialize () {
		DontDestroyOnLoad (gameObject);
		Advertisement.Initialize (GAME_ID);
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
