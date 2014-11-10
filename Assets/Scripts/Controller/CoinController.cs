using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

	public GameObject getCoinEffectPrefab;
	private float mLifeTime = 3.0f;

	void Update () {
		mLifeTime -= Time.deltaTime;
		if (mLifeTime < 0) {
			Destroy (transform.parent.gameObject);
		}
	}

	public void OnClick () {
		GameObject getCoinEffectObject = Instantiate (getCoinEffectPrefab) as GameObject;
		getCoinEffectObject.transform.parent = transform.parent.transform.parent;
		getCoinEffectObject.transform.localScale = new Vector3 (1f,1f,1f);
		getCoinEffectObject.transform.localPosition = transform.localPosition;
		Destroy (transform.parent.gameObject);
	}
}
