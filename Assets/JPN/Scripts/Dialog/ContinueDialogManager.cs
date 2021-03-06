﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Advertisements;

public class ContinueDialogManager : MonoSingleton<ContinueDialogManager> {

	public static event Action FinishPuzzleEvent;
	public static event Action BuyTapCountEvent;

	public GameObject buyTapButtonObject;
	public GameObject watchUnityAdsButtonFilter;
	public UIGrid grid;
	private GameObject mDialogObject;
	private GameObject mFenceObject;

	public override void OnInitialize () {
		mDialogObject = transform.FindChild ("Dialog").gameObject;
		mFenceObject = transform.FindChild ("Fence").gameObject;
	}

	void CompleteDismissEvent () {
		mDialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	public void WatchMovieButtonClicked () {
		UnityAdsManager.instance.ShowAd ();
	}

	public void OnFinishPuzzleClicked () {
		mFenceObject.SetActive (false);
		FinishPuzzleEvent ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnBuyTapCountClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if (PlayerDataKeeper.instance.TicketCount < 1) {
			OKDialog.instance.OnOKButtonClicked = () => {
				BuyTicketDialog.instance.Show ();
			};
			OKDialog.instance.Show ("チケットが不足しています");
			return;
		}
		mFenceObject.SetActive (false);
		BuyTapCountEvent ();
		PlayerDataKeeper.instance.DecreaseTicketCount (1);
	}

	public void Show () {
		mDialogObject.SetActive (true);
		mFenceObject.SetActive (true);
		UpdateUnityAdsButtonFilter ();
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}

	public void Dismiss () {
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
	}

	public void FinishedUnityAds (ShowResult result) {
		UpdateUnityAdsButtonFilter ();
		if (result == ShowResult.Finished) {
			mFenceObject.SetActive (false);
			BuyTapCountEvent ();
		}
	}

	private void UpdateUnityAdsButtonFilter () {
		if (UnityAdsManager.instance.IsReady ()) {
			watchUnityAdsButtonFilter.SetActive (false);
		} else {
			watchUnityAdsButtonFilter.SetActive (true);
		}
	}
}
