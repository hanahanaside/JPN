using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	public GameObject adObject;
	private int mIndex;

	enum Days{Sun = 5,Mon,Tue};

	void Start(){
		Debug.Log ("start child");
	}

	public void ButtonClicked(){
		Debug.Log ("click");
		Destroy (adObject);
	}
}
