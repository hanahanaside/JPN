using UnityEngine;
using System.Collections;

public class AdManager : MonoSingleton<AdManager> {

	private string mPublisherId = "5222";
	private string mRectanglelMediaId = "117652";
	private string mRectangleSpotId = "283376";
	private int mRectangleViewId;

	public override void OnInitialize(){
		Debug.Log ("zss");
		IMobileSdkAdsUnityPlugin.registerInline (mPublisherId,mRectanglelMediaId,mRectangleSpotId);
		IMobileSdkAdsUnityPlugin.start (mRectangleSpotId);
		mRectangleViewId = IMobileSdkAdsUnityPlugin.show (mRectangleSpotId,IMobileSdkAdsUnityPlugin.AdType.MEDIUM_RECTANGLE,0,0);
	}

	public void ShowRectangleAd(){
		IMobileSdkAdsUnityPlugin.setVisibility (mRectangleViewId,true);
	}

	public void HideRectangleAd(){
		IMobileSdkAdsUnityPlugin.setVisibility (mRectangleViewId,false);
	}
}
