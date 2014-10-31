using UnityEngine;
using System.Collections;

public class IdleNormalState : IdleState {

	private float mFlightDuration;
	//滞空時間
	private float mJumpForce;
	//ジャンプパワー
	private float mMoveForce;
	//横の移動距離
	private float mStopTime;
	private IdleData mIdleData;

	public IdleNormalState (IdleData idleData) {
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
		Debug.Log ("jump = " + mJumpForce);
		rigidbody2D.AddForce (Vector2.right * mMoveForce);
		//ここで動いたり止まったりを調整する
		mJumpForce = Random.Range (mIdleData.jumpForce / 1.6f, mIdleData.jumpForce * 1.5f);
		//	mStopTime = Random.Range(0,mFlightDuration * 3.0f);
	}

	public void Stop () {
		//ストップ中でなければストップ開始
		if (mStopTime <= 0) {
			mStopTime = Random.Range (1.5f, 2.0f);
		}
	}

	public void DirectionUp () {
		mJumpForce = mIdleData.jumpForce * 2.0f;
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
