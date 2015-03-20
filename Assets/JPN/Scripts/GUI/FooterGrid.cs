using UnityEngine;
using System.Collections;

public class FooterGrid : MonoBehaviour {

	void Start () {
		#if UNITY_ANDROID
		transform.localPosition = new Vector3 (0, -440, 0);
		#endif
	}
	
}
