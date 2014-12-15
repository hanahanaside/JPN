using UnityEngine;
using System.Collections;
using System;

public class Puzzle : MonoBehaviour {

	public static event Action<string> OpenedPuzzleEvent;
	public int[] firstIndexArray = { 1, 2, 3, 6, 7, 8, 11, 12, 13, 16, 17, 18 };
	public int[] rangeArray = { 4, 6 };

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
