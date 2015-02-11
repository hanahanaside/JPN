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
		IMobileSdkAdsUnityPlugin.show (ImobileAccount.INTERSTITIAL_SPOT_ID);
	}

	public void HideBannerAd () {
		IMobileSdkAdsUnityPlugin.setVisibility (mBannerViewId, false);
	}

	public void ShowBannerAd () {
		IMobileSdkAdsUnityPlugin.setVisibility (mBannerViewId, true);
	}

	public void HideIconAd () {
		IMobileSdkAdsUnityPlugin.setVisibility (mIconViewId, false);
	}

	public void ShowIconAd () {
		IMobileSdkAdsUnityPlugin.setVisibility (mIconViewId, true);
	}

	private void InitBannerAd () {
		IMobileSdkAdsUnityPlugin.registerInline (ImobileAccount.PUBLISHER_ID, ImobileAccount.BANNER_MEDIA_ID, ImobileAccount.BANNER_SPOT_ID);
		IMobileSdkAdsUnityPlugin.start (ImobileAccount.BANNER_SPOT_ID);
		mBannerViewId =	IMobileSdkAdsUnityPlugin.show (ImobileAccount.BANNER_SPOT_ID,
			IMobileSdkAdsUnityPlugin.AdType.BANNER,
			IMobileSdkAdsUnityPlugin.AdAlignPosition.CENTER,
			IMobileSdkAdsUnityPlugin.AdValignPosition.BOTTOM);
		HideBannerAd ();
	}

	private void InitIconAd () {
		IMobileSdkAdsUnityPlugin.registerInline (ImobileAccount.PUBLISHER_ID, ImobileAccount.ICON_MEDIA_ID, ImobileAccount.ICON_SPOT_ID);
		IMobileSdkAdsUnityPlugin.start (ImobileAccount.ICON_SPOT_ID);
		IMobileIconParams iconParams = new IMobileIconParams ();
		iconParams.iconNumber = 4;
		iconParams.iconTitleEnable = false;
		mIconViewId = IMobileSdkAdsUnityPlugin.show (ImobileAccount.ICON_SPOT_ID, IMobileSdkAdsUnityPlugin.AdType.ICON, 0, 355, iconParams);
		HideIconAd ();
	}

	private void InitInterstitialAd () {
		IMobileSdkAdsUnityPlugin.registerFullScreen (ImobileAccount.PUBLISHER_ID, ImobileAccount.INTERSTITIAL_MEDIA_ID, ImobileAccount.INTERSTITIAL_SPOT_ID);
		IMobileSdkAdsUnityPlugin.start (ImobileAccount.INTERSTITIAL_SPOT_ID);
	}
}
