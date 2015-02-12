using UnityEngine;
using System.Collections;

public class AdManager :  MonoSingleton<AdManager> {

	private int mBannerViewId;
	private int mIconViewId;

	public override void OnInitialize () {
		DontDestroyOnLoad (gameObject);
		#if !UNITY_EDITOR
		InitBannerAd ();
		InitIconAd ();
		InitInterstitialAd ();
		#endif
	}

	public void ShowInterstitialAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.show (ImobileAccount.INTERSTITIAL_SPOT_ID);
		#endif
	}

	public void HideBannerAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.setVisibility (mBannerViewId, false);
		#endif
	}

	public void ShowBannerAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.setVisibility (mBannerViewId, true);
		#endif
	}

	public void HideIconAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.setVisibility (mIconViewId, false);
		#endif
	}

	public void ShowIconAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.setVisibility (mIconViewId, true);
		#endif
	}

	private void InitBannerAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.registerInline (ImobileAccount.PUBLISHER_ID, ImobileAccount.BANNER_MEDIA_ID, ImobileAccount.BANNER_SPOT_ID);
		IMobileSdkAdsUnityPlugin.start (ImobileAccount.BANNER_SPOT_ID);
		mBannerViewId =	IMobileSdkAdsUnityPlugin.show (ImobileAccount.BANNER_SPOT_ID,
			IMobileSdkAdsUnityPlugin.AdType.BANNER,
			IMobileSdkAdsUnityPlugin.AdAlignPosition.CENTER,
			IMobileSdkAdsUnityPlugin.AdValignPosition.BOTTOM);
		HideBannerAd ();
		#endif
	}

	private void InitIconAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.registerInline (ImobileAccount.PUBLISHER_ID, ImobileAccount.ICON_MEDIA_ID, ImobileAccount.ICON_SPOT_ID);
		IMobileSdkAdsUnityPlugin.start (ImobileAccount.ICON_SPOT_ID);
		IMobileIconParams iconParams = new IMobileIconParams ();
		iconParams.iconNumber = 4;
		iconParams.iconTitleEnable = false;
		mIconViewId = IMobileSdkAdsUnityPlugin.show (ImobileAccount.ICON_SPOT_ID, IMobileSdkAdsUnityPlugin.AdType.ICON, 0, 355, iconParams);
		HideIconAd ();
		#endif
	}

	private void InitInterstitialAd () {
		#if !UNITY_EDITOR
		IMobileSdkAdsUnityPlugin.registerFullScreen (ImobileAccount.PUBLISHER_ID, ImobileAccount.INTERSTITIAL_MEDIA_ID, ImobileAccount.INTERSTITIAL_SPOT_ID);
		IMobileSdkAdsUnityPlugin.start (ImobileAccount.INTERSTITIAL_SPOT_ID);
		#endif
	}
}
