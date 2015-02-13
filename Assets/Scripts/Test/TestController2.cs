using UnityEngine;
using System.Collections;

public class TestController2 : MonoBehaviour {

	void OnEnable(){
		Debug.Log ("enabele");
	}

	// Use this for initialization
	void Start () {

	}
	
	public void Hoge(){
		gameObject.SetActive (true);
	}
}
