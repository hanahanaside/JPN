using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public UILabel label;

	void Start(){
		int[] test =  PrefsManager.instance.ClearedPuzzleCountArray;
		Debug.Log ("" + test);
	}

}
