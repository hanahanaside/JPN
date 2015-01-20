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
	private static extern void sruPassBannerN (int tag, int frame_id, int[] gravity,int grav_length, int top, int bottom, int left, int right);
	
	[DllImport("__Internal")]
	private static extern void sruPassIconN (int tag, int frame_id, int[] gravity,int grav_length, int top, int bottom, int left, int right);
	
	[DllImport("__Internal")]
	private static extern void destroyAdN (int tag);

	
	public static void sruPassPrepare(string media_id,bool debug){
		sruPassPrepareN(media_id,debug);
	}

	public static void sruPassInterstitial(int frame_id){
		sruPassInterstitialN(frame_id);
	}

	public static void sruPassBanner(int tag, int frame_id, int[] gravity, int top, int bottom, int left, int right){
		sruPassBannerN(tag, frame_id, gravity, gravity.Length, top, bottom, left, right);
	}

	public static void sruPassIcon(int tag, int frame_id, int[] gravity, int top, int bottom, int left, int right){
		sruPassIconN(tag, frame_id, gravity, gravity.Length, top, bottom, left, right);
	}

	public static void destroyAd(int tag){
		destroyAdN(tag);
	}
}