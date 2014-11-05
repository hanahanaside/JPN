using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Rigidbody))]

public class TestController : MonoBehaviour {

	public Idle idle;
	public Fan fan;

	public void OnButtonClicked(){
		fan.StartDancing ();
	}
}
