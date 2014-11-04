using UnityEngine;
using System.Collections;

public class HokkaidoIdle : Idle {

	private float mTime;
	private IdleState mState;

	void Start () {
		mState = new HokkaidoIdleNormalState (normalIdleParam);
		mTime = mState.FlightDuration();
		mState.DirectionLeft ();
		mState.Move (gameObject);
	}
	void Update () {
		mTime -= Time.deltaTime;
		if (mTime > 0) {
			return;
		}
		Move (mState);
		mTime = mState.FlightDuration();
	}

	public override void Sleep(){

	}

	public override void WakeUp(){

	}

	public override void StartDancing(){
		Debug.Log ("dance");
		mState = new HokkaidoIdleDanceState (danceIdleParam);
		mTime = mState.FlightDuration();
		mState.Move (gameObject);
	}
}
