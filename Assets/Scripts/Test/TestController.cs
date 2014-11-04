using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Rigidbody))]

public class TestController : MonoBehaviour {

	public Idle idle;

	public void OnButtonClicked(){
		idle.StartDancing ();
	}
}
