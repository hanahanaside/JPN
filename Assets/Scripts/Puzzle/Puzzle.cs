using UnityEngine;
using System.Collections;
using System;

public class Puzzle : MonoBehaviour {

	public static event Action<string> OpenedPuzzleEvent;
	private UIButton mButton;

	void Start () {
		mButton = GetComponent<UIButton> ();
		int rand = UnityEngine.Random.Range (1, 5);
		mButton.normalSprite = "puzzle_base_" + rand;
	}

	void OnClick () {
		collider.enabled = false;
		mButton.normalSprite = tag;
		OpenedPuzzleEvent (tag);
	}
}
