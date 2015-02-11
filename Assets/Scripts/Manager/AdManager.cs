using UnityEngine;
using System.Collections;

public class AdManager :  MonoSingleton<AdManager> {

	public UICenterOnChild uiCenterOnChild;
	private GameObject mIconAd;
	private bool mIconAdshowing;

	public override void OnInitialize () {
		uiCenterOnChild.onCenter += OnCenterCallBack;
		mIconAd = transform.FindChild ("IconAd").gameObject;
	}
		
	void OnCenterCallBack (GameObject centeredObject) {
		if (centeredObject.tag == "sleep") {
			ShowIconAd ();
		} else {
			HideIconAd ();
		}
	}

	public void ShowInterstitialAd(){

	}

	public void HideBannerAd(){

	}

	public void HideIconAd () {
		mIconAd.BroadcastMessage ("OnDestroy");
		mIconAdshowing = false;
	}

	private void ShowIconAd () {
		if(!mIconAdshowing){
			mIconAd.BroadcastMessage ("Show");
			mIconAdshowing = true;
		}
	}

}
