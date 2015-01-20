using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System;
using ObjC = ObjCMessage;


public interface SuruPassAdBannerCallback 
{
}

public class SuruPassAdBanner : SuruPassAd 
{
	private static int sTagNumber = 2;

	[SerializeField]
	protected Metrics androidMetrics;
	[SerializeField]
	protected Metrics iOSMetrics;

	private static SuruPassAdBanner sInstance;

	public override void OnInitialize(){
		sInstance = this;
		sTagNumber++;
		tagNumber = sTagNumber;
	}

	public static SuruPassAdBanner instance{
		get{
			return sInstance;
		}
	}

	public override void Show()
	{
		if(tagNumber != 0){
#if UNITY_IPHONE && !UNITY_EDITOR
		ObjC.sruPassPrepare(account.iOS.media_id,account.debug);
		int[] result = Array.ConvertAll(iOSMetrics.gravity, value => (int) value);
		ObjC.sruPassBanner(tagNumber, account.iOS.frame_id, result, iOSMetrics.margin.left, iOSMetrics.margin.top, iOSMetrics.margin.right, iOSMetrics.margin.bottom);
#elif UNITY_ANDROID && !UNITY_EDITOR
		_ShowBanner(gameObject.name,androidMetrics.gravity);
#endif
		}
	}

#if UNITY_IPHONE && !UNITY_EDITOR

#elif UNITY_ANDROID && !UNITY_EDITOR
	private void _ShowBanner(string gameObject,  Gravity[] gravity) {
		int g_sum = 0;
		foreach (Gravity g in gravity){
			g_sum |=  (int)g;
		}
		adUtil.CallStatic("showBanner", tagNumber, account.android.frame_id, g_sum, androidMetrics.margin.left, androidMetrics.margin.top, androidMetrics.margin.right, androidMetrics.margin.bottom); 
	}
#endif
}