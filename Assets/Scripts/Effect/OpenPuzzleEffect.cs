using UnityEngine;
using System.Collections;

public class OpenPuzzleEffect : MonoBehaviour {

	public GameObject getCoinEffect;
	private double mAddCoin;

	public void StartAnimation (double addCoin, string spriteName) {
		mAddCoin = addCoin;
		GetComponentInChildren<UISprite> ().spriteName = spriteName;
		Debug.Log ("name " +spriteName);
		Hashtable hashTable = new Hashtable ();
		hashTable.Add ("x", 0);
		hashTable.Add ("y", 1);
		hashTable.Add ("z", 0);
		hashTable.Add ("speed", 2);
		hashTable.Add ("easetype", iTween.EaseType.linear);
		hashTable.Add ("oncomplete", "CompleteHandler");
		iTween.MoveTo (gameObject, hashTable);
	}

	private void CompleteHandler () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
		GameObject getCoinEffectObject = Instantiate (getCoinEffect) as GameObject;
		getCoinEffectObject.transform.parent = transform.parent;
		getCoinEffectObject.transform.localScale = new Vector3 (1, 1, 1);
		getCoinEffectObject.transform.localPosition = transform.localPosition;
		PlayerDataKeeper.instance.IncreaseCoinCount (mAddCoin);
		Destroy (gameObject);
	}
}
