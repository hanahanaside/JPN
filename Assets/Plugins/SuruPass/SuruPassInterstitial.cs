using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using ObjC = ObjCMessage;
using System;


public class SuruPassInterstitial : SuruPassAd 
{

	private static SuruPassInterstitial sInstance;

	public override void OnInitialize(){
		sInstance = this;
		DontDestroyOnLoad(gameObject);
	}

	public static SuruPassInterstitial instance{
		get{
			return sInstance;
		}
	}

	public override void Show()
	{
		#if UNITY_IPHONE && !UNITY_EDITOR
		ObjC.sruPassPrepare(account.iOS.media_id,account.debug);
		ObjC.sruPassInterstitial(account.iOS.frame_id);
		#elif UNITY_ANDROID && !UNITY_EDITOR
		adUtil.CallStatic("show",   account. android.frame_id);
		#endif
	}
	
}