using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestController : TestParent {

	public iTweenEvent tweenEvent;

	void Start () {
		Debug.Log ("" + PlayerPrefs.GetInt("gfdgd"));
	}

	void Update () {


	}

	public void OnButtonClicked () {
		tweenEvent.Play ();
	}
}
