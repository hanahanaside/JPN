using UnityEngine;
using System.Collections;


public class TestController : TestParent {
	public GameObject adObject;
	public UILabel label;
	private int mIndex;


	enum Days{Sun = 5,Mon,Tue};

	void Start(){
		SuruPassInterstitial.instance.Show ();
	}

	public void ButtonClicked(){
		Debug.Log ("click");
		SuruPassAdBanner.instance.Hide ();
	}

	public void FinishedTypeWriter(){
		Debug.Log ("finished");
	}
}
