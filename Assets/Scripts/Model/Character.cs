using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

	protected enum Direction {
		Left,
		Right,
		Up,
		Down
	}

	private Transform transform;

	void Awake(){
		transform = transform;
	}
}