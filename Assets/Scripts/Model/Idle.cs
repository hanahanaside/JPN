using UnityEngine;
using System.Collections;

public class Idle : Character {

	public IdleData normalIdleData;
	public IdleData danceIdleData;
	private IdleState mState;
	private float mTime;

	void Start () {
		mState = new HokkaidoIdleNormalState (gameObject,normalIdleData);
		mTime = mState.FlightDuration ();
		mState.DirectionLeft ();
		mState.Move (gameObject);
	}

	void Update () {
		mTime -= Time.deltaTime;
		if (mTime > 0) {
			return;
		}
		if (characterTransform.localPosition.x > 200.0f) {
			transform.eulerAngles = new Vector3 (0, 0, 0);
			mState.Stop ();
			mState.DirectionLeft ();
		}
		if (characterTransform.localPosition.x < -200.0f) {
			transform.eulerAngles = new Vector3 (0, 180.0f, 0);
			mState.Stop ();
			mState.DirectionRight ();
		}
		if (characterTransform.localPosition.y > 480.0f) {
			mState.DirectionDown ();
			mState.Stop ();
		}
		if (characterTransform.localPosition.y < -480.0f) {
			mState.DirectionUp ();
			mState.Stop ();
		}
		mState.Move (gameObject);
		mTime = mState.FlightDuration ();
	}

	public void WakeUp(){

	}

	public void Sleep(){

	}

	public override void StartDancing(){
		mState = new HokkaidoIdleDanceState ();
		mTime = mState.FlightDuration ();
		mState.Move (gameObject);
	}
}
