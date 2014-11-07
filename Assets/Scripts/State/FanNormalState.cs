using UnityEngine;
using System.Collections;

public class FanNormalState : FanState {

	private Fan mFan;
	private iTweenEvent mRotateEvent;
	private float mTime;
	private bool mStop;

	public FanNormalState (Fan fan,iTweenEvent rotateEvent) {
		mFan = fan;
		mRotateEvent = rotateEvent;
		mTime = Random.Range (1.0f,3.0f);
		mRotateEvent.Play ();
	}

	public void Move () {
		mTime -= Time.deltaTime;
		if (mStop) {
			if (mTime < 0) {
				mStop = false;
				mRotateEvent.Play ();
				mTime = Random.Range (1.0f,3.0f);
			}
			return;
		}
		mFan.CheckFlip ();
		mFan.Move ();
		if (mTime < 0) {
			mStop = true;
			mRotateEvent.Stop ();
			mFan.ResetRotation ();
			mFan.ChangeDirection ();
			mTime = Random.Range (1.0f,3.0f);
		}
	}
}
