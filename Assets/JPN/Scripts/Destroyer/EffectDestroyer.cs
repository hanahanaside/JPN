using UnityEngine;
using System.Collections;

public class EffectDestroyer : MonoBehaviour {

	public float lifeTime;

	void Start () {
		Invoke ("DestroyMySelf", lifeTime);
	}

	private void DestroyMySelf () {
		Destroy (gameObject);
	}
}
