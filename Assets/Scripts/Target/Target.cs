using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Target : MonoBehaviour {

	public static event Action<string> CompletePuzzleEvent;
	public static event Action UpdateGameEvent;
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
		if(puzzleTag == tag){
			Correct ();
		}
		if(mCorrectCount >= mChildList.Count){
			iTweenEvent.GetEvent (gameObject,"ExitEvent").Play();
		}else {
			UpdateGameEvent ();
		}
	}

	void CompleteExitEvent(){
			CompletePuzzleEvent (tag);
			Destroy (gameObject);
	}

	private void Correct(){
		mCorrectCount++;
		UISprite sprite = mChildList[mCorrectCount-1].GetComponent<UISprite>();
		sprite.spriteName = "symbol_on";
	}
}
