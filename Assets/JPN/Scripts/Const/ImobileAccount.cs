using UnityEngine;
using System.Collections;

public static class ImobileAccount {

	#if UNITY_IPHONE
	public const string PUBLISHER_ID = "5222";
	public const string BANNER_MEDIA_ID = "137749";
	public const string BANNER_SPOT_ID = "374588";
	public const string ICON_MEDIA_ID = "137749";
	public const string ICON_SPOT_ID = "374591";
	public const string INTERSTITIAL_MEDIA_ID = "137749";
	public const string INTERSTITIAL_SPOT_ID = "374592";
	public const string RECTANGLE_MEDIA_ID = "137749";
	public const string RECTANGLE_SPOT_ID = "350198";
	#endif

	#if UNITY_ANDROID
	public const string PUBLISHER_ID = "5222";
	public const string BANNER_MEDIA_ID = "151279";
	public const string BANNER_SPOT_ID = "392984";
	public const string ICON_MEDIA_ID = "151279";
	public const string ICON_SPOT_ID = "392985";
	public const string INTERSTITIAL_MEDIA_ID = "151279";
	public const string INTERSTITIAL_SPOT_ID = "392986";
	public const string RECTANGLE_MEDIA_ID = "151279";
	public const string RECTANGLE_SPOT_ID = "407276";
	#endif

}
