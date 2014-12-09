using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Target : MonoBehaviour {

	public static event Action<string> CompleteTargetEvent;

	private List<Transform> mChildList;
	private int mCorrectCount;

	void Start () {
		mChildList = GetComponentInChildren<UIGrid> ().GetChildList ();
	}

	void CompleteExitEvent(){
		Destroy (gameObject);
		CompleteTargetEvent (tag);
	}
		
	public void Correct () {
		mCorrectCount++;
		UISprite sprite = mChildList [mCorrectCount - 1].GetComponent<UISprite> ();
		sprite.spriteName = "symbol_on";
	}

	public bool CheckNotComplete(){
		if (mCorrectCount >= mChildList.Count) {
			return false;
		}
		return true;
	}

	public void StartCompleteEvent(){
		iTweenEvent.GetEvent (gameObject, "ExitEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetIdol_1);
	}
}
