using UnityEngine;
using System.Collections;
using System;

public class CoinController : MonoBehaviour {

	public static event Action<string> OnClickedEvent;
	public TweenColor tweenColor;
	public GameObject getCoinEffectPrefab;
	private float mLifeTime = 6.0f;

	void Start () {
		float x = UnityEngine.Random.Range (-220.0f, 220.0f);
		float y = UnityEngine.Random.Range (-150.0f,  300.0f);
		transform.localPosition = new Vector3 (x,y,0);
		Vector3[] movePath = new Vector3[3];
		movePath [0] = new Vector3 (x, y, 0);
		movePath [1] = new Vector3 (x, y + 150.0f, 0);
		movePath [2] = new Vector3 (x, y, 0);
		iTween.MoveTo (gameObject, iTween.Hash ("path", movePath, "time", 0.8, "easetype", iTween.EaseType.easeOutQuint, "islocal", true,"movetopath",false));
	}

	void Update () {
		mLifeTime -= Time.deltaTime;
		if (mLifeTime < 0) {
			Destroy (gameObject);
		}
	}

	public void OnClick () {
		string tag = gameObject.tag;
		OnClickedEvent (tag);
		GameObject getCoinEffectObject = Instantiate (getCoinEffectPrefab) as GameObject;
		getCoinEffectObject.transform.parent = transform.parent;
		getCoinEffectObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		getCoinEffectObject.transform.localPosition = transform.localPosition;
		Destroy (gameObject);
	}
}
