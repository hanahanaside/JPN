using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class ObjCMessage 
{
	[DllImport("__Internal")]
	private static extern void sruPassPrepareN (string str,bool debug);

	[DllImport("__Internal")]
	private static extern void sruPassInterstitialN (int param);

	[DllImport("__Internal")]
	private static extern void sruPassBannerN (int frame_id, int[] gravity, int top, int bottom, int left, int right);
	
	[DllImport("__Internal")]
	private static extern void sruPassIconN (int frame_id, int[] gravity, int top, int bottom, int left, int right);
	
	public static void sruPassPrepare(string media_id,bool debug){
		sruPassPrepareN(media_id,debug);
	}

	public static void sruPassInterstitial(int frame_id){
		sruPassInterstitialN(frame_id);
	}

	public static void sruPassBanner(int frame_id, int[] gravity, int top, int bottom, int left, int right){
		sruPassBannerN(frame_id, gravity, top, bottom, left, right);
	}

	public static void sruPassIcon(int frame_id, int[] gravity, int top, int bottom, int left, int right){
		sruPassIconN(frame_id, gravity, top, bottom, left, right);
	}

}