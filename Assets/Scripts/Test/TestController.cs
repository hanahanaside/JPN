using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestController : TestParent {

	public TestController2 hoge;
	public UILabel label;

	void Start () {
		Debug.Log ("" + PlayerPrefs.GetInt("gfdgd"));
	}

	void Update () {


	}

	public void OnButtonClicked () {
		hoge.Hoge ();
	}
}
