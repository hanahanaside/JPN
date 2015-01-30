using UnityEngine;
using System.Collections;

public class HanautaCamera : MonoSingleton<HanautaCamera> {

	private Vector3 mPosition;

	public override void OnInitialize () {
		mPosition = transform.position;
	}

	public Vector3 Postision {
		get {
			return mPosition;
		}
	}
}
