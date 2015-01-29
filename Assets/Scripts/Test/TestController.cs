using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	public GameObject iconAdPrefab;
	private bool mShowing = false;

	void Start () {
	//	mIconAd = Instantiate (iconAdPrefab) as GameObject;
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit ();
		}

	}

	public void OnButtonClicked () {
	
		if(mShowing){
				iconAdPrefab.SendMessage ("OnDestroy");
			mShowing = false;
		}else {
			iconAdPrefab.SendMessage ("Show");
			mShowing = true;
		}

	}
}
