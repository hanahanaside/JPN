using UnityEngine;
using System.Collections;

public class IdleNormalState : IdleState {

	private float mTime;
	private Idle mIdle;
	private bool mStop;
	private iTweenEvent mMoveEvent;

	public IdleNormalState(Idle idle,iTweenEvent moveEvent){
		mIdle = idle;
		mMoveEvent = moveEvent;
	}

	public void Move(){
		mTime -= Time.deltaTime;
		if (mStop) {
			if (mTime < 0) {
				mStop = false;
				mMoveEvent.Play ();
				mTime = Random.Range (1.0f,3.0f);
			}
			return;
		}
		mIdle.CheckFlip ();
		mIdle.Move ();
		if (mTime < 0) {
			mStop = true;
			mMoveEvent.Stop ();
			mIdle.ChangeDirection ();
			mTime = Random.Range (1.0f,3.0f);
		}
	}
}
