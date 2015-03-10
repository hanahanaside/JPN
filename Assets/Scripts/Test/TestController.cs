using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using MiniJSON;

public class TestController : TestParent {

	public UILabel label_1;
	public UILabel label_2;

	void Awake () {
		label_1.overflowMethod = UILabel.Overflow.ResizeHeight;
		Vector3 label_1Position = label_1.transform.localPosition;
		Debug.Log (label_1Position);

	}
		
	void Start(){

	//	label_1.text = "a";
		Vector3 label_1Position = label_1.transform.localPosition;
		int height = label_1.height;
		float label_2_y = label_1Position.y - height;
		label_2.transform.localPosition = new Vector3 (0,label_2_y,0);
	}

}
