using UnityEngine;
using System.Collections;
using System;

public class Idle : Character {
		
	public static event Action<Character> FoundEvent;

	private string idleId;

	private float mTime;
	private State mState = State.Move;
	private iTweenEvent mJumpEvent;
	private iTweenEvent mScaleEvent;
	private iTweenEvent mRotateEvent;
	private iTweenEvent mIdleEvent;
	private bool jump;

	public void Init () {
		idleId = name.Replace ("Idle_", "");
		idleId = idleId.Replace ("(Clone)", "");
		mJumpEvent = iTweenEvent.GetEvent (gameObject, "JumpEvent");
		mIdleEvent = iTweenEvent.GetEvent (gameObject, "IdleEvent");
		mScaleEvent = iTweenEvent.GetEvent (sprite.gameObject, "ScaleEvent");
		mRotateEvent = iTweenEvent.GetEvent (sprite.gameObject, "RotateEvent");
		characterTransform.localScale = new Vector3 (0.8f,0.8f,0.8f);
		ResizeSprite ();
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
				mIdleEvent.Stop ();
				transform.localEulerAngles = new Vector3 (0, 0, 0);
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
		sprite.transform.localScale = new Vector3 (1f, 1f, 1f);
		mIdleEvent.Play ();
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
		mIdleEvent.Stop ();
		mJumpEvent.Stop ();
		mScaleEvent.Stop ();
		sprite.spriteName = "idle_sleep_" + idleId;
		sprite.transform.localScale = new Vector3 (1f, 1f, 1f);
		transform.localEulerAngles = new Vector3 (0, 0, 0);
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
		sprite.spriteName = "idle_normal_" + idleId;
		ResizeSprite ();
		//迷子中でなかったらSpriteを消す
		if(collider == null){
			sprite.enabled = false;
		}
	}

	public override void FinishLive () {
		sprite.enabled = true;
		mState = State.Move;
		mRotateEvent.Stop ();
		sprite.transform.localEulerAngles = new Vector3 (0, 0, 0);
	}

	public override void StartMoving () {
		mState = State.Move;
		sprite.spriteName = "idle_normal_" + idleId;
		ResizeSprite ();
		mTime = moveTimeSeconds;
		ChangeDirection (CheckDirection ());
		mJumpEvent.Play ();
		mScaleEvent.Play ();
	}

	public void OnClick () {
		FoundEvent (this);
		GameObject effectPrefab = Resources.Load ("Effect/GetCoinEffect") as GameObject;
		GameObject effectObject = Instantiate (effectPrefab) as GameObject;
		effectObject.transform.parent = characterTransform.parent;
		effectObject.transform.localScale = new Vector3 (1,1,1);
		effectObject.transform.localPosition = characterTransform.localPosition;
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Katsu);
		Destroy (gameObject);
	}

	private void ResizeSprite () {
		UISpriteData spriteData = sprite.GetAtlasSprite ();
		sprite.width = (int)(spriteData.width / 1.3);
		sprite.height = (int)(spriteData.height / 1.3);
	}
}