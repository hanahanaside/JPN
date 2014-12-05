using UnityEngine;
using System.Collections;
using System;

public class GetIdleDialogManager : MonoBehaviour {

	public static event Action<string> ClosedEvent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCloseButtonClicked(){
		ClosedEvent (tag);
		Destroy (transform.parent.gameObject);
	}
}
