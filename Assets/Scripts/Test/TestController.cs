using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	public NewsEvent newsEvent;

	void Start(){
		Debug.Log ("" + newsEvent.occurring);
		newsEvent.occurring = false;
	}
}
