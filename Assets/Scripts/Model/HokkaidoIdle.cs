using UnityEngine;
using System.Collections;

public class HokkaidoIdle : Idle {

	private float mTime;
	private bool mSleep;
	private IdleState mState;

	void Start () {
		mState = new HokkaidoIdleNormalState (normalIdleParam);
		mTime = mState.FlightDuration();
		mState.DirectionLeft ();
		mState.Move (gameObject);
	}

	void Update () {
		if(mSleep){
			return;
		}
		mTime -= Time.deltaTime;
		if (mTime > 0) {
			return;
		}
		Move (mState);
		mTime = mState.FlightDuration();
	}

	public override void Sleep(){
		MyLog.LogDebug ("sleep");
		mSleep = true;
		IsKinematic = true;
		SetSprite ("idle_sleep_1");
	}

	public override void WakeUp(){
		SetSprite ("idle_normal_1");
		mSleep = false;
		mState = new HokkaidoIdleNormalState (normalIdleParam);
		mState.DirectionLeft ();
		mState.Move (gameObject);
	}

	public override void StartDancing(){
		MyLog.LogDebug ("dance");
		mState = new HokkaidoIdleDanceState (danceIdleParam);
		mTime = mState.FlightDuration();
		mState.Move (gameObject);
	}
}
