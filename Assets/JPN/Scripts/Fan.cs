using UnityEngine;
using System.Collections;

public class Fan : Human {

	// Use this for initialization
	void Start () {
		Debug.Log ("Child Start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Move ()
	{

	}

	public override void Dance ()
	{

	}

	private void OnTriggerEnter(Collider colider)
	{
		if(colider.tag == "box"){
			Debug.Log ("aaaa");
		}

	}
}
