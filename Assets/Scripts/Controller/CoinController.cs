using UnityEngine;
using System.Collections;
using System;

public class CoinController : MonoBehaviour {

	public static event Action<string> OnClickedEvent;

	public GameObject getCoinEffectPrefab;
	private float mLifeTime = 3.0f;

	void Start(){
		float x = UnityEngine.Random.Range(-220,220);
		float y = UnityEngine.Random.Range(-180,-60);
	}

	void Update () {
		mLifeTime -= Time.deltaTime;
		if (mLifeTime < 0) {
			Destroy (transform.parent.gameObject);
		}
	}

	public void OnClick () {
		string tag = gameObject.tag;
		OnClickedEvent (tag);
		GameObject getCoinEffectObject = Instantiate (getCoinEffectPrefab) as GameObject;
		getCoinEffectObject.transform.parent = transform.parent.transform.parent;
		getCoinEffectObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		getCoinEffectObject.transform.localPosition = transform.localPosition;
		Destroy (transform.parent.gameObject);
	}
}
