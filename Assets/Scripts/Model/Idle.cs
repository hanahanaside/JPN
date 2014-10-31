using UnityEngine;
using System.Collections;

public class Idle : Character {

	public MovableArea movableArea;
	public IdleData normalIdleData;
	public IdleData danceIdleData;
	private IdleState mState;
	private float mTime;

	void Start () {
		mState = new IdleNormalState (normalIdleData);
		mTime = normalIdleData.flightDuration;
		mState.DirectionLeft ();
		mState.Move (gameObject);
	}

	void Update () {
		mTime -= Time.deltaTime;
		if (mTime > 0) {
			return;
		}
		if (characterTransform.localPosition.x > movableArea.limitRight) {
			transform.eulerAngles = new Vector3 (0, 0, 0);
			mState.Stop ();
			mState.DirectionLeft ();
		}
		if (characterTransform.localPosition.x < movableArea.limitLeft) {
			transform.eulerAngles = new Vector3 (0, 180.0f, 0);
			mState.Stop ();
			mState.DirectionRight ();
		}
		if (characterTransform.localPosition.y > movableArea.limitTop) {
			mState.Stop ();
			mState.DirectionDown ();
		}
		if (characterTransform.localPosition.y < movableArea.limitBottom) {
			mState.Stop ();
			mState.DirectionUp ();
		}
		mState.Move (gameObject);
		mTime = mState.FlightDuration ();
	}

	public void WakeUp(){

	}

	public void Sleep(){

	}

	public override void StartDancing(){
		mState = new IdleDanceState ();
		mTime = mState.FlightDuration ();
		mState.Move (gameObject);
	}
}
