using UnityEngine;
using System.Collections;

public class Idle : Character {
		
	public int idleId;

	private float mTime;
	private State mState = State.Move;
	private iTweenEvent mJumpEvent;
	private iTweenEvent mScaleEvent;
	private iTweenEvent mRotateEvent;
	private bool jump;
	private UISprite mSprite;

	public void Init () {
		mSprite = transform.FindChild ("Sprite").GetComponent<UISprite> ();
		mJumpEvent = iTweenEvent.GetEvent (gameObject, "JumpEvent");
		mScaleEvent = iTweenEvent.GetEvent (gameObject, "ScaleEvent");
		mRotateEvent = iTweenEvent.GetEvent (mSprite.gameObject, "RotateEvent");
		StartMoving ();
	}

	void Update () {
		mTime -= Time.deltaTime;
		switch (mState) {
		//ムーブ
		case State.Move:
			characterTransform.Translate (new Vector3 (moveSpeed.speedX, moveSpeed.speedY, 0));
			break;
		//ストップ
		case State.Stop:
			//動きを再開
			if (mTime < 0) {
				mState = State.Move;
				mTime = moveTimeSeconds;
				ChangeDirection (CheckDirection ());
				mJumpEvent.Play ();
				mScaleEvent.Play ();
			}
			break;
		//ライブ
		case State.Live:
			if (CheckLimit ()) {
				ChangeDirection (CheckDirection ());
			}
			characterTransform.Translate (new Vector3 (moveSpeed.speedX, moveSpeed.speedY, 0));
			break;
		//スリープ
		case State.Sleep:
			break;
		}
	}

	public override void Stop () {
		mState = State.Stop;
		mTime = stopTimeSeconds;
		mJumpEvent.Stop ();
		mScaleEvent.Stop ();
		if(moveSpeed.speedX > 0){
			characterTransform.localScale = new Vector3 (-1f, 1f, 1f);
		}else {
			characterTransform.localScale = new Vector3 (1f, 1f, 1f);
		}

	}

	void OnCompleteJumpEvent () {
		jump = !jump;
		if (mState != State.Move) {
			return;
		}
		if (jump) {
			return;
		}
		if (CheckLimit ()) {
			Stop ();
		}
		if (mTime < 0) {
			Stop ();
		}
	}
		
	public override void Sleep () {
		mState = State.Sleep;
		mJumpEvent.Stop ();
		mScaleEvent.Stop ();
		mSprite.spriteName = "idle_sleep_" + idleId;
		ResizeSprite ();
	}

	public override void Wakeup () {
		StartMoving ();
	}

	public override void StartLive () {
		mState = State.Live;
		ChangeDirection (CheckDirection ());
		mJumpEvent.Play ();
		mScaleEvent.Play ();
		mRotateEvent.Play ();
		mSprite.spriteName = "idle_normal_" + idleId;
		ResizeSprite ();
	}

	public override void FinishLive () {
		mState = State.Move;
		mRotateEvent.Stop ();
		mSprite.transform.localEulerAngles = new Vector3 (0, 0, 0);
	}

	public override void StartMoving(){
		mState = State.Move;
		mSprite.spriteName = "idle_normal_" + idleId;
		ResizeSprite ();
		mTime = moveTimeSeconds;
		ChangeDirection (CheckDirection ());
		mJumpEvent.Play ();
		mScaleEvent.Play ();
	}

	private void ResizeSprite () {
		UISpriteData spriteData = mSprite.GetAtlasSprite ();
		mSprite.SetDimensions (spriteData.width, spriteData.height);
	}
}