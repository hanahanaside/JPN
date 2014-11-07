using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public Fan fan;

	void Start(){

	}

	public void OnButtonClicked(){
		Debug.Log ("click");
		fan.StartLive ();
	}
}
