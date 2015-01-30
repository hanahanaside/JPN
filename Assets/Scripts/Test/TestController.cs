using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	public TestController2 hoge;

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
		hoge.Hoge ();
	}
}
