using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	public TestController2 hoge;

	void Start () {

	}

	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}

	}

	public void OnButtonClicked () {
		hoge.Hoge ();
	}
}
