using UnityEngine;
using System.Collections;


public class TestController : TestParent {
	public GameObject adObject;
	public UILabel label;
	private int mIndex;


	enum Days{Sun = 5,Mon,Tue};

	void Start(){
		string dateString = "2014/10/12";
		System.DateTime dt = System.DateTime.Parse (dateString);
		Debug.Log ("aaa " +dt);
	}
		
	public void ButtonClicked(){
		if(!UnityAdsManager.instance.IsReady()){
			Debug.Log ("not ready");
			return;
		}
		UnityAdsManager.instance.ShowAd ();
	}

	public void FinishedTypeWriter(){
		Debug.Log ("finished");
	}
}
