using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class TestController : TestParent {

	public iTweenEvent tweenEvent;

	void Start () {
	
	}

	void Update () {


	}

	public void OnButtonClicked () {
		tweenEvent.Play ();
	}

}
