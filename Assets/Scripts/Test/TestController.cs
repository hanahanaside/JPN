using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestController : TestParent {

	public TestController2 hoge;
	public UILabel label;

	void Start () {
	//	label.text = "111111111111111111";
	}

	void Update () {


	}

	public void OnButtonClicked () {
		hoge.Hoge ();
	}
}
