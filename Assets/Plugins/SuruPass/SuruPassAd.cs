using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using ObjC = ObjCMessage;

public abstract class SuruPassAd : MonoBehaviour
{
	[SerializeField]
	protected Account account;
	[SerializeField]
	protected bool automaticDisplay = true;
	[SerializeField]
	protected bool outputLog = false;
	[SerializeField]
	protected int tagNumber = 0;


#if UNITY_ANDROID && !UNITY_EDITOR
	protected AndroidJavaClass adUtil;
#endif

	[System.SerializableAttribute]
	protected enum Gravity : int 
	{
		LEFT = 3,
		TOP = 48,
		RIGHT = 5,
		BOTTOM = 80,
		CENTER_VERTICAL = 16,
		CENTER_HORIZONTAL = 1
	}

	[System.SerializableAttribute]
	protected class Margin 
	{
		public int left = 0;
		public int top = 0;
		public int right = 0;
		public int bottom = 0;
	}

	[System.SerializableAttribute]
	protected class Account
	{
		public bool debug;
		public SuruPassID android;
		public SuruPassID iOS;
	}
	
	[System.SerializableAttribute]
	protected class SuruPassID
	{
		public string media_id;
		public int frame_id;
	}

	[System.SerializableAttribute]
	protected class Metrics
	{
		public Margin margin;
		public Gravity[] gravity;
	}

	/// <summary>
	/// Show this instance.
	/// </summary>
	public abstract void Show();

	void Awake()
	{

		#if !UNITY_EDITOR
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		DontDestroyOnLoad(gameObject);
		#endif

		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		adUtil = new AndroidJavaClass("tokyo.aid.unity.Adutil");
		adUtil.CallStatic("init", activity, account.android.media_id, account.debug);
		#endif

		OnInitialize ();
	}
	
	// Use this for initialization
	void Start()
	{

		if ( automaticDisplay ) 
		{
			#if !UNITY_EDITOR
			Show();
			#endif
		}

	}
	
	void Update()
	{
		
	}

	void OnDestroy()
	{
		#if UNITY_IPHONE && !UNITY_EDITOR
		ObjC.destroyAd(tagNumber);
		#elif UNITY_ANDROID && !UNITY_EDITOR
		adUtil.CallStatic("destroyAd", tagNumber);
		#endif
		Debug.Log ("tag number " +tagNumber);
	}
	
	protected int GetBitGravity(Gravity[] gravity)
	{
		int bit = 0;
		foreach ( int flag in gravity ) 
		{
			bit |= flag;
		}
		return bit;
	}
		
	public void Hide(){
		Destroy (gameObject);
	}

	public virtual void OnInitialize(){}
}