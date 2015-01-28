using UnityEngine;
using System.Collections;

public class AdManager :  MonoSingleton<AdManager> {

	public GameObject bannerAdPrefab;
	public GameObject iconAdPrefab;
	private GameObject mBannerAdObject;
	private GameObject mIconAdObject;
	public UICenterOnChild uiCenterOnChild;

	public override void OnInitialize () {
		mBannerAdObject = Instantiate (bannerAdPrefab) as GameObject;
		uiCenterOnChild.onCenter += OnCenterCallBack;
	}

	public override void OnFinalize () {
		Destroy (mBannerAdObject);
	}

	void OnCenterCallBack (GameObject centeredObject) {
		if (centeredObject.tag == "sleep") {
			ShowIconAd ();
		} else {
			HideIconAd ();
		}
	}

	public void HideIconAd () {
		if (mIconAdObject != null) {
			Destroy (mIconAdObject);
			mIconAdObject = null;
		}
	}

	private void ShowIconAd () {
		if (mIconAdObject == null) {
			mIconAdObject = Instantiate (iconAdPrefab) as GameObject;
		}
	}

}
