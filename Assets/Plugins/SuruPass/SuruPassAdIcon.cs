using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System;
using ObjC = ObjCMessage;


public class SuruPassAdIcon : SuruPassAd 
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
		ObjC.sruPassIcon(account.iOS.frame_id,result, iOSMetrics.margin.left, iOSMetrics.margin.top, iOSMetrics.margin.right, iOSMetrics.margin.bottom);
#elif UNITY_ANDROID && !UNITY_EDITOR
		_ShowIcons(gameObject.name, GetBitGravity(androidMetrics.gravity));
#endif
	}


#if UNITY_IPHONE && !UNITY_EDITOR
#elif UNITY_ANDROID && !UNITY_EDITOR

	private void _ShowIcons(string gameObject, int gravity) { 
		adUtil.CallStatic("showIcons", account.android.frame_id, gravity,  androidMetrics.margin.left, androidMetrics.margin.top, androidMetrics.margin.right, androidMetrics.margin.bottom ); 
	}

#endif
}