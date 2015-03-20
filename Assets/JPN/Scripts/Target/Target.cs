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
		for (int i = 0; i < symbolCount; i++) {
			GameObject symbolObject = Instantiate (symbolPrefab)as GameObject;
			grid.AddChild (symbolObject.transform);
			symbolObject.transform.localScale = new Vector3 (1, 1, 1);
		}
		mChildList = grid.GetChildList ();
		UISprite sprite = GetComponent<UISprite> ();
		sprite.depth = 2;
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider> ();
		boxCollider.size = new Vector3 (sprite.width, sprite.height, 0);
		boxCollider.isTrigger = true;
		gameObject.AddComponent<UIButtonScale> ();
	}

	void CompleteExitEvent () {
		FenceManager.instance.HideTransparentFence ();
		enabled = false;
		CompleteTargetEvent (tag);
		Destroy (gameObject);
	} 

	void OnClick () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		string tag = gameObject.tag;
		if (tag == "ticket") {
			return;
		}
		int id = Convert.ToInt32 (tag.Replace ("idle_", ""));
		MapDialogManager.instance.Show (id);
	}

	public void Correct () {
		if (!enabled) {
			return;
		}
		mCorrectCount++;
		UISprite sprite = mChildList [mCorrectCount - 1].GetComponent<UISprite> ();
		sprite.spriteName = "symbol_on";
		if (mCorrectCount >= mChildList.Count) {
			FenceManager.instance.ShowTransparentFence ();
			iTweenEvent.GetEvent (gameObject, "ExitEvent").Play ();
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetIdol_1);
		} else {
			UpdateGameEvent ();
		}
	}
}
