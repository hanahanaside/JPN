using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public GameObject uiRoot;
	public GameObject puzzle;

	void Start(){
		GameObject a =  Instantiate (puzzle) as GameObject;
		UISprite sprite = a.GetComponent<UISprite> ();
	sprite.spriteName = "puzzle_2";
	//	a.transform.parent = uiRoot.transform;
	}
}
