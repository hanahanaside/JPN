using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public abstract class Character : MonoBehaviour {

	public abstract void StartDancing();
}
