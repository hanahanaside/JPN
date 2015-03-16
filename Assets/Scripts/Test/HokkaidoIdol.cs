using UnityEngine;
using System.Collections;
using System;

public class HokkaidoIdol : Character {

	public static event Action<Character> FoundEvent;

	private int idleId = 1;

	private float mTime;
	private State mState = State.Move;
	private iTweenEvent mJumpEvent;
	private iTweenEvent mScaleEvent;
	private iTweenEvent mIdleEvent;
	private bool jump;

	private IdolState mIdolState;

	void Awake () {
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		StartMoving ();
	}

	public override void Stop () {
		mIdolState = new HokkaidoIdolIdleState (this);
		sprite.transform.localScale = new Vector3 (1f, 1f, 1f);
		mIdolState.Move (gameObject);
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
		StartMoving ();
	}

	//ライブを開始
	public override void StartLive () {
		bool isLost = (collider != null);
		if (isLost) {
			mState = State.Move;
		} else {
			mState = State.Live;
			gameObject.SetActive (false);
		}
	}

	//ライブを終了
	public override void FinishLive () {
		gameObject.SetActive (true);
		mState = State.Move;
		sprite.spriteName = "idle_normal_" + idleId;
		ResizeSprite ();
		mTime = moveTimeSeconds;
		ChangeDirection (CheckDirection ());
		if (transform.parent.gameObject.activeSelf) {
			mJumpEvent.Play ();
			mScaleEvent.Play ();
		}
	}

	//動き出す
	public override void StartMoving () {
		sprite.spriteName = "idle_normal_" + idleId;
		ResizeSprite ();
		mIdolState = new HokkaidoIdolWalkState (this);
		mIdolState = mIdolState.Move (gameObject);
	}

	void MoveFinished () {
		mIdolState = mIdolState.Move (gameObject);
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
