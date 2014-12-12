using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Target : MonoBehaviour {

	public static event Action UpdateGameEvent;
	public static event Action<string> CompleteTargetEvent;

	public int puzzleCount;
	public GameObject symbolPrefab;

	private List<Transform> mChildList;
	private int mCorrectCount;

	void Start () {
		for(int i = 0;i < puzzleCount;i++){
			GameObject symbolObject = Instantiate (symbolPrefab) as GameObject;
			GetComponentInChildren<UIGrid> ().AddChild (symbolObject.transform);
			symbolObject.transform.localScale = new Vector3 (1,1,1);
		}
		GetComponentInChildren<UIGrid> ().Reposition ();
		mChildList = GetComponentInChildren<UIGrid> ().GetChildList ();
	}

	void CompleteExitEvent(){
		enabled = false;
		CompleteTargetEvent (tag);
	}
		
	public void Correct () {
		if(!enabled){
			return;
		}
		mCorrectCount++;
		UISprite sprite = mChildList [mCorrectCount - 1].GetComponent<UISprite> ();
		sprite.spriteName = "symbol_on";
		if (mCorrectCount >= mChildList.Count) {
			iTweenEvent.GetEvent (gameObject, "ExitEvent").Play ();
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetIdol_1);
		}else {
			UpdateGameEvent ();
		}
	}
}
