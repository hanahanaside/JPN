using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Target : MonoBehaviour {

	public static event Action UpdateGameEvent;
	public static event Action<string> CompleteTargetEvent;
	public int symbolCount;
	public GameObject symbolPrefab;

	private List<Transform> mChildList;
	private int mCorrectCount;

	void Start () {
		UIGrid grid = GetComponentInChildren<UIGrid> ();
		for(int i = 0;i<symbolCount;i++){
			GameObject symbolObject = Instantiate (symbolPrefab)as GameObject;
			grid.AddChild (symbolObject.transform);
			symbolObject.transform.localScale = new Vector3 (1,1,1);
		}
		mChildList = grid.GetChildList ();
		GetComponent<UISprite> ().depth = 2;
	}

	void CompleteExitEvent(){
		FenceManager.instance.HideTransparentFence ();
		enabled = false;
		Destroy (gameObject);
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
			FenceManager.instance.ShowTransparentFence ();
			iTweenEvent.GetEvent (gameObject, "ExitEvent").Play ();
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetIdol_1);
		}else {
			UpdateGameEvent ();
		}
	}
}
