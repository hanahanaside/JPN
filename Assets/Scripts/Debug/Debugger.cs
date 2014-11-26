using UnityEngine;
using System.Collections;

public class Debugger : MonoBehaviour {

	public GameObject soundManagerPrefab;

	#if UNITY_EDITOR
	void Awake () {
		if (GameObject.FindGameObjectWithTag ("SoundManager") == null) {
			Instantiate (soundManagerPrefab);
		}
	}
	#endif
}
