using UnityEngine;
using System.Collections;

public class HanautaUIButton : MonoBehaviour {

	public bool disableOnClick;

	void OnEnable () {
		if (!collider.enabled) {
			collider.enabled = true;
		}
	}

	void OnClick () {
		if (disableOnClick) {
			collider.enabled = false;
		}
	}

}
