using UnityEngine;
using System.Collections;

public class Fan : Character {

	public GameObject spriteObject;

	private FanState mFanState;
	private iTweenEvent[] mDanceEventArray;

	void Start () {
		base.characterTransform = transform;
		mFanState = new FanNormalState (this, iTweenEvent.GetEvent (spriteObject, "RotateEvent"));
	}

	void Update () {
		mFanState.Move ();
	}
		
	public override void StartDancing () {
		mFanState = new FanDanceState (this);
		iTweenEvent.GetEvent (gameObject, "MoveEvent").Play ();
		iTweenEvent.GetEvent (gameObject, "ScaleEvent").Play ();
	}

	public override void StopDancing () {
		iTweenEvent.GetEvent (gameObject, "MoveEvent").Stop ();
		iTweenEvent.GetEvent (gameObject, "ScaleEvent").Stop ();
		ResetRotation ();
		mFanState = new FanNormalState (this, iTweenEvent.GetEvent (spriteObject, "RotateEvent"));
	}
		
	public void ResetRotation () {
		spriteObject.transform.eulerAngles = new Vector3 (0, 0, 0);
	}
}
