using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class WWWClient {
	
	public delegate void RequestFinishedDelegate (string response);

	public delegate void TimeOutDelegate ();

	private const float TIME_OUT_INTERVAL = 10.0f;
	private RequestFinishedDelegate mOnSuccess;
	private RequestFinishedDelegate mOnFail;
	private TimeOutDelegate mOnTimeOut;
	private MonoBehaviour mMonoBehaviour;
	private WWW mWWW;
	private Dictionary<string,string> mHeader;
	private string mURL;
	private string mPostData;
	private bool mIsTimeOut;

	public WWWClient (MonoBehaviour monoBehaviour, string url) {
		mMonoBehaviour = monoBehaviour;
		mURL = url;
		mHeader = new Dictionary<string, string> ();
	}

	public RequestFinishedDelegate OnSuccess {
		set{ mOnSuccess = value; }
	}

	public RequestFinishedDelegate OnFail {
		set{ mOnFail = value; }
	}

	public TimeOutDelegate OnTimeOut {
		set{ mOnTimeOut = value; }
	}

	public string PostData {
		set { mPostData = value; }
	}

	public void AddJsonHeader () {
		mHeader.Add ("Content-Type", "application/json");
	}

	public void Request () {
		mMonoBehaviour.StartCoroutine (RequestCoroutine ());
	}

	private IEnumerator RequestCoroutine () {
		if (string.IsNullOrEmpty (mPostData)) {
			//http get
			mWWW = new WWW (mURL);
		} else if (mHeader.Count == 0) {
			//http post
			byte[] postByteData = Encoding.UTF8.GetBytes (mPostData);
			mWWW = new WWW (mURL, postByteData);
		} else {
			//http post
			byte[] postByteData = Encoding.UTF8.GetBytes (mPostData);
			mWWW = new WWW (mURL, postByteData, mHeader);
		}
		yield return mMonoBehaviour.StartCoroutine (CheckTimeout ());

		if (mIsTimeOut && mOnTimeOut != null) {
			Debug.Log ("TimeOut");
			mOnTimeOut ();
		} else if (mWWW.error == null && mOnSuccess != null) {
			Debug.Log ("www ok");
			Debug.Log ("result = " + mWWW.text);
			mOnSuccess (mWWW.text);
		} else if (mOnFail != null) {
			Debug.Log ("www error");
			Debug.Log (mWWW.text);
			mOnFail (mWWW.text);
		}
		mWWW.Dispose ();
	}

	private  IEnumerator CheckTimeout () {
		float startRequestTime = Time.time;
		while (!mWWW.isDone) {
			if (Time.time - startRequestTime < TIME_OUT_INTERVAL)
				yield return null;
			else {
				//タイムアウト
				mIsTimeOut = true;
				break;
			}
		}
		yield return null;
	}
}
