using UnityEngine;
using System.Collections;

public class HokkaidoIdleDanceState : IdleState {

	private float mFlightDuration;
	//滞空時間
	private float mJumpForce;
	//ジャンプパワー
	private float mMoveForce;
	//横の移動距離
	private float mStopTime;
	private IdleParameter mIdleData;

	public HokkaidoIdleDanceState (IdleParameter idleData) {
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
		iTweenEvent.GetEvent (gameobject,"DanceMove").Play();
		rigidbody2D.AddForce (Vector2.right * mMoveForce);
		//ここで次の動きを決定する
		float addJumpForce = mIdleData.jumpForce / 2.0f;
		mJumpForce = Random.Range (mIdleData.jumpForce - addJumpForce, mIdleData.jumpForce + addJumpForce);
		mMoveForce = Random.Range (-mIdleData.moveForce, mIdleData.moveForce);
	}

	public void Stop () {
		mStopTime = -1;
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
