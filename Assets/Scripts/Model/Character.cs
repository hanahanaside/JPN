using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public abstract class Character : MonoBehaviour {

	[HideInInspector]
	public Transform characterTransform;

	void Awake () {
		characterTransform = transform;
	}

	public abstract void StartDancing();
}
