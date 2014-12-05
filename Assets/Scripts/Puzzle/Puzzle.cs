using UnityEngine;
using System.Collections;
using System;

public class Puzzle : MonoBehaviour {

	public static event Action<string> OpenedPuzzleEvent;

	public string puzzleTag;
	private UIButton mButton;

	void Start () {
		mButton = GetComponent<UIButton> ();
		int rand = UnityEngine.Random.Range (1, 5);
		mButton.normalSprite = "puzzle_base_" + rand;
	}

	void OnClick () {
		collider.enabled = false;
		mButton.normalSprite = "puzzle_" + puzzleTag;
		OpenedPuzzleEvent (puzzleTag);
	}
}
