using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour {

	public MovableArea movableArea;
	public GameObject spriteObject;
	public float moveSpeedX;
	public float moveSpeedY;

	private FanState mFanState;
	private Transform mTransform;
	private iTweenEvent[] mDanceEventArray;

	void Start () {
		mTransform = transform;
		mFanState = new FanNormalState (this, iTweenEvent.GetEvent (spriteObject, "RotateEvent"));
	}

	void Update () {
		mFanState.Move ();
	}

	public void CheckFlip () {
		if (mTransform.localPosition.x < movableArea.limitLeft) {
			moveSpeedX = -moveSpeedX;
		}
		if (mTransform.localPosition.x > movableArea.limitRight) {
			moveSpeedX = -moveSpeedX;
		}
		if (mTransform.localPosition.y < movableArea.limitBottom) {
			moveSpeedY = -moveSpeedY;
		}
		if (mTransform.localPosition.y > movableArea.limitTop) {
			moveSpeedY = -moveSpeedY;
		}
	}

	public void ChangeDirection () {
		int rand = Random.Range (0, 4);
		switch (rand) {
		case 0:
			break;
		case 1:
			moveSpeedX = -moveSpeedX;
			break;
		case 2:
			moveSpeedY = -moveSpeedY;
			break;
		case 3:
			moveSpeedX = -moveSpeedX;
			moveSpeedY = -moveSpeedY;
			break;
		}
	}

	public void StartDancing () {
		mFanState = new FanDanceState (this);
		iTweenEvent.GetEvent (gameObject, "MoveEvent").Play ();
		iTweenEvent.GetEvent (gameObject, "ScaleEvent").Play ();
	}

	public void StopDancing () {
		iTweenEvent.GetEvent (gameObject, "MoveEvent").Stop ();
		iTweenEvent.GetEvent (gameObject, "ScaleEvent").Stop ();
		ResetRotation ();
		mFanState = new FanNormalState (this, iTweenEvent.GetEvent (spriteObject, "RotateEvent"));
	}

	public void Move () {
		mTransform.Translate (new Vector3 (moveSpeedX, moveSpeedY, 0));
	}

	public void ResetRotation () {
		spriteObject.transform.eulerAngles = new Vector3 (0, 0, 0);
	}
}
