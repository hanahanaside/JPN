using UnityEngine;
using System.Collections;

public class HokkaidoIdleNormalState : IdleState {

	private float mFlightDuration;
	//滞空時間
	private float mJumpForce;
	//ジャンプパワー
	private float mMoveForce;
	//横の移動距離
	private float mStopTime;
	private IdleParameter mIdleData;

	public HokkaidoIdleNormalState (IdleParameter idleData) {
		mIdleData = idleData;
		mFlightDuration = idleData.flightDuration;
		mJumpForce = idleData.jumpForce;
		mMoveForce = idleData.moveForce;
	}
		
	//滞空時間ごとに呼ばれる
	public void Move (GameObject gameobject) {
		Rigidbody2D rigidbody2D = gameobject.rigidbody2D;
		mStopTime -= mFlightDuration;
		if (mStopTime > 0) {
			rigidbody2D.isKinematic = true;
			return;
		}
		rigidbody2D.isKinematic = false;
		rigidbody2D.velocity = Vector2.up * mJumpForce;
		if (mMoveForce > 0) {
			gameobject.transform.eulerAngles = new Vector3 (0, 180, 0);
		} else {
			gameobject.transform.eulerAngles = new Vector3 (0, 0, 0);
		}
		gameobject.transform.localScale = new Vector3 (1f, 1f, 1f);
		iTweenEvent.GetEvent (gameobject, "NormaMove").Play ();
		rigidbody2D.AddForce (Vector2.right * mMoveForce);
		//ここで次の動きを決定する
		float addJumpForce = mIdleData.jumpForce / 2.0f;
		mJumpForce = Random.Range (mIdleData.jumpForce - addJumpForce, mIdleData.jumpForce + addJumpForce);
		mStopTime = Random.Range (0, mFlightDuration * 3.0f);
		mMoveForce = Random.Range (-mIdleData.moveForce, mIdleData.moveForce);
	}

	public void Stop () {
		//ストップ中でなければストップ開始
		if (mStopTime <= 0) {
			mStopTime = Random.Range (1.5f, 2.0f);
		}
	}

	public void DirectionUp () {
		mJumpForce = mIdleData.jumpForce * 1.5f;
	}

	public void DirectionDown () {
		mJumpForce = mIdleData.jumpForce / 2.0f;
	}

	public void DirectionLeft () {
		if (mMoveForce > 0) {
			mMoveForce = -mMoveForce;
		}
	}

	public void DirectionRight () {
		if (mMoveForce < 0) {
			mMoveForce = -mMoveForce;
		}
	}

	public float FlightDuration () {
		return mFlightDuration;
	}
}
