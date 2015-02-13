using UnityEngine;
using System.Collections;

public class Worker : Character {

	private iTweenEvent mRotateEvent;
	private iTweenEvent mJumpEvent;
	private State mState;
	private float mTime;
	private bool mDancing;

	public void Init () {
		mRotateEvent = iTweenEvent.GetEvent (sprite.gameObject, "RotateEvent");
		mJumpEvent = iTweenEvent.GetEvent (gameObject, "JumpEvent");
		StartMoving ();
	}

	void Update () {
		mTime -= Time.deltaTime;
		switch (mState) {
		//ムーブ
		case State.Move:
			if(mDancing){
				StopDancing ();
				StartMoving ();
			}
			characterTransform.Translate (new Vector3 (moveSpeed.speedX, moveSpeed.speedY, 0));
			//座標上限をチェック
			if (CheckLimit ()) {
				Stop ();
			}
			//可動時間をチェック
			if (mTime < 0) {
				Stop ();
			}
			break;
		//ストップ
		case State.Stop:
			if (mTime < 0) {
				StartMoving ();
			}
			break;
		//ライブ
		case State.Live:
			if(!mDancing){
				StartDancing ();
			}
			break;
		//スリープ
		case State.Sleep:
			break;
		}
	}

	public override void StartLive () {
		mState = State.Live;
		mDancing = false;
	}

	public override void FinishLive () {
		mState = State.Move;
	}

	public override void Sleep () {
		mState = State.Sleep;
	}

	public override void Wakeup () {
		mState = State.Move;
	}

	public override void Stop () {
		sprite.transform.localEulerAngles = new Vector3 (0, 0, 0);
		mState = State.Stop;
		mTime = stopTimeSeconds;
		mRotateEvent.Stop ();
	}

	public override void StartMoving () {
		mRotateEvent.Play ();
		mState = State.Move;
		mTime = moveTimeSeconds;
		ChangeDirection (CheckDirection ());
	}


	//踊りを開始
	private void StartDancing(){
		mRotateEvent.Stop ();
		sprite.transform.localEulerAngles = new Vector3 (0, 0, 0);
		mJumpEvent.Play ();
		mDancing = true;
	}

	//踊りを中止
	private void StopDancing(){
		mJumpEvent.Stop ();
		mDancing = false;
	}
		
}
