using UnityEngine;
using System.Collections;

public class TestController2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	public void Hoge(){
		gameObject.SetActive (true);
	}
}
