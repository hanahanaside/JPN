using UnityEngine;
using System.Collections;

public class Fan : Character {

	private iTweenEvent mRotateEvent;
	private iTweenEvent mJumpEvent;
	private State mState;
	private float mTime;

	public void Init () {
		mRotateEvent = iTweenEvent.GetEvent (sprite.gameObject, "RotateEvent");
		mJumpEvent = iTweenEvent.GetEvent (gameObject, "JumpEvent");
		characterTransform.localScale = new Vector3 (0.8f,0.8f,0.8f);
	}

	void Update () {
		mTime -= Time.deltaTime;
		switch (mState) {
		//ムーブ
		case State.Move:
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
			break;
		//スリープ
		case State.Sleep:
			mRotateEvent.Stop ();
			break;
		}
	}

	public override void StartLive () {
		mState = State.Live;
		mRotateEvent.Stop ();
		sprite.transform.localEulerAngles = new Vector3 (0, 0, 0);
		mJumpEvent.Play ();
	}

	public override void FinishLive () {
		mJumpEvent.Stop ();
		StartMoving ();
	}

	public override void Sleep () { 
		mState = State.Sleep;
	}

	public override void Wakeup () {
		StartMoving ();
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
		if(characterTransform.localPosition.y >= movableArea.limitTop){
			characterTransform.localPosition = new Vector3 (characterTransform.localPosition.x,movableArea.limitTop - 5.0f,0);
		}
		if(characterTransform.localPosition.y <= movableArea.limitBottom){
			characterTransform.localPosition = new Vector3 (characterTransform.localPosition.x,movableArea.limitBottom +  5.0f,0);
		}
	}

}
