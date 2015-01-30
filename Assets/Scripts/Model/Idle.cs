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
	private iTweenEvent mIdleEvent;
	private bool jump;
	private bool mDancing;

	public void Init () {
		idleId = name.Replace ("Idle_", "");
		idleId = idleId.Replace ("(Clone)", "");
		mJumpEvent = iTweenEvent.GetEvent (gameObject, "JumpEvent");
		mIdleEvent = iTweenEvent.GetEvent (gameObject, "IdleEvent");
		mScaleEvent = iTweenEvent.GetEvent (sprite.gameObject, "ScaleEvent");
		characterTransform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		ResizeSprite ();
		StartMoving ();
	}

	void Update () {
		mTime -= Time.deltaTime;
		switch (mState) {
		//ムーブ
		case State.Move:
			//ダンスの状態だったら解除する
			if(mDancing){
				StartMoving ();
				mDancing = false;
			}
			//移動
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

	//ジャンプ終了時に呼ばれる
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

	//サボる
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

	//起きる
	public override void Wakeup () {
		mState = State.Move;
		StartMoving ();
	}

	//ライブを開始
	public override void StartLive () {
		mState = State.Live;
		mDancing = true;
		//迷子中でなかったらSpriteを消す
		if (collider == null) {
			gameObject.SetActive (false);
		}
	}

	//ライブを終了
	public override void FinishLive () {
		mState = State.Move;
		gameObject.SetActive (true);
	}
		
	//動き出す
	public override void StartMoving () {
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
		effectObject.transform.localScale = new Vector3 (1, 1, 1);
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