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

	[SerializeField]
	protected Metrics androidMetrics;
	[SerializeField]
	protected Metrics iOSMetrics;

	public override void Show()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		ObjC.sruPassPrepare(account.iOS.media_id,account.debug);
		int[] result = Array.ConvertAll(iOSMetrics.gravity, value => (int) value);
		ObjC.sruPassBanner(account.iOS.frame_id, result, iOSMetrics.margin.left, iOSMetrics.margin.top, iOSMetrics.margin.right, iOSMetrics.margin.bottom);
#elif UNITY_ANDROID && !UNITY_EDITOR
		_ShowBanner(gameObject.name,androidMetrics.gravity);
#endif
	}

#if UNITY_IPHONE && !UNITY_EDITOR

#elif UNITY_ANDROID && !UNITY_EDITOR
	private void _ShowBanner(string gameObject,  Gravity[] gravity) {
		int g_sum = 0;
		foreach (Gravity g in gravity){
			g_sum |=  (int)g;
		}
		adUtil.CallStatic("showBanner",  account.android.frame_id, g_sum, androidMetrics.margin.left, androidMetrics.margin.top, androidMetrics.margin.right, androidMetrics.margin.bottom); 
	}
#endif
}