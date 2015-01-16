using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using ObjC = ObjCMessage;
using System;


public class SuruPassInterstitial : SuruPassAd {

	public static SuruPassInterstitial sInstance;

	public static SuruPassInterstitial instance{
		get{
			return sInstance;
		}
	}

	public override void OnInitialize(){
		sInstance = this;
	}

	public override void Show () {
		#if UNITY_IPHONE && !UNITY_EDITOR
		ObjC.sruPassPrepare(account.iOS.media_id,account.debug);
		ObjC.sruPassInterstitial(account.iOS.frame_id);
		#elif UNITY_ANDROID && !UNITY_EDITOR
		adUtil.CallStatic("show",   account. android.frame_id);
		#endif
	}
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	
	
#elif UNITY_ANDROID && !UNITY_EDITOR
	private void _ShowBanner(string gameObject) { 
		adUtil.CallStatic("showBanner",  account.android.frame_id); 
	}
	#endif
}