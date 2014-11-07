using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

	private float mLifeTime = 3.0f;

	void Update () {
		mLifeTime -= Time.deltaTime;
		if (mLifeTime < 0) {
			Destroy (characterTtransform.parent.gameObject);
		}
	}

	public void OnClick () {
		Destroy (characterTtransform.parent.gameObject);
	}
}
