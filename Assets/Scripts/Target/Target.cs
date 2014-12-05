using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Target : MonoBehaviour {

	public string targetTag;
	private List<Transform> mChildList;
	private int mCorrectCount;

	void OnEnable(){
		Puzzle.OpenedPuzzleEvent += OpenedPuzzleEvent;
	}

	void OnDisable(){
		Puzzle.OpenedPuzzleEvent -= OpenedPuzzleEvent;
	}

	void Start(){
		mChildList = GetComponentInChildren<UIGrid> ().GetChildList();
	}

	void OpenedPuzzleEvent(string puzzleTag){
		if(puzzleTag == targetTag){
			Correct ();
		}
		if(mCorrectCount >= mChildList.Count){
			Debug.Log ("get");
		}
	}

	private void Correct(){
		mCorrectCount++;
		UISprite sprite = mChildList[mCorrectCount-1].GetComponent<UISprite>();
		sprite.spriteName = "symbol_on";
	}
}
