using UnityEngine;
using System.Collections;

public abstract class Human : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("parent start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void Move();

	public abstract void Dance();
}
