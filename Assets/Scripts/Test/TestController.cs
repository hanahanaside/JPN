using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	public GameObject iconAdPrefab;
	private GameObject mIconAd;

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
		if (mIconAd == null) {
			mIconAd = Instantiate (iconAdPrefab) as GameObject;
			Debug.Log ("instantiate");
		} else {
			Destroy (mIconAd);
			Debug.Log ("destroy");
		}
	}
}
