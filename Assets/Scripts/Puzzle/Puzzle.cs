using UnityEngine;
using System.Collections;
using System;

public class Puzzle : MonoBehaviour {

	public static event Action<GameObject> OpenedPuzzleEvent;

	public int[] firstIndexArray;
	public int[] rangeArray;
	private GameObject mOpenEffectPrefab;

	private UIButton mButton;

	void Start () {
		mButton = GetComponent<UIButton> ();
		int[] baseIdArray = { 1, 1, 1, 1, 1, 1, 1, 2, 3, 4 };
		int rand = UnityEngine.Random.Range (0, baseIdArray.Length);
		int baseId = baseIdArray [rand];
		mButton.normalSprite = "puzzle_base_" + baseId;
		mOpenEffectPrefab = Resources.Load ("Effect/GetCoinEffect") as GameObject;
	}

	void OnClick () {
		Open ();
		OpenedPuzzleEvent (gameObject);
	}

	public void Open(){
		GameObject effectObject =  Instantiate (mOpenEffectPrefab) as GameObject;
		effectObject.transform.parent = gameObject.transform;
		effectObject.transform.localScale = new Vector3 (1,1,1);
		effectObject.transform.localPosition = new Vector3 (0,0,0);
		collider.enabled = false;
		mButton.normalSprite = "puzzle_" + tag;
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Katsu);
	}


}
