using UnityEngine;
using System.Collections;
using System;

public class Puzzle : MonoBehaviour {

	public static event Action<string> OpenedPuzzleEvent;

	public int[] firstIndexArray;
	public int[] rangeArray;

	private UIButton mButton;

	void Start () {
		mButton = GetComponent<UIButton> ();
		int[] baseIdArray = { 1, 1, 1, 1, 1, 1, 1, 2, 3, 4 };
		int rand = UnityEngine.Random.Range (0, baseIdArray.Length);
		int baseId = baseIdArray [rand];
		mButton.normalSprite = "puzzle_base_" + baseId;
	}

	void OnClick () {
		collider.enabled = false;
		mButton.normalSprite = "puzzle_" + tag;
		OpenedPuzzleEvent (tag);
	}
}
