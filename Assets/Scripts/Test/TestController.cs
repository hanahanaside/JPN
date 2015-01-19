using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	public UILabel label;
	private int mIndex;

	enum Days{Sun = 5,Mon,Tue};

	void Start(){
		Debug.Log ("start child");
	}

	public void ButtonClicked(){
		label.text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
		label.GetComponent<TypewriterEffect> ().ResetToBeginning ();

	}

	public void FinishedTypeWriter(){
		Debug.Log ("finished");
	}
}
