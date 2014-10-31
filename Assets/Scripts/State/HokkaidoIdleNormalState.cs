using UnityEngine;
using System.Collections;

public class HokkaidoIdleNormalState : IdleState {

	private const float FLIGHT_DURATION = 1.0f; //滞空時間
	private float mJumpForce = 1.0f; //ジャンプパワー
	private float mMoveForce = 5.0f; //横の移動距離
	private float mStopTime;

	public HokkaidoIdleNormalState(GameObject gameobject,IdleData idleData){

	}

	//滞空時間ごとに呼ばれる
	public void Move (GameObject gameobject) {
		Rigidbody2D rigidbody2D = gameobject.rigidbody2D;
		mStopTime -= FLIGHT_DURATION;
		MyLog.LogDebug ("time = " + mStopTime);
		if (mStopTime > 0) {
			rigidbody2D.isKinematic = true;
			return;
		}
		rigidbody2D.isKinematic = false;
		rigidbody2D.velocity = Vector2.up * mJumpForce;
		rigidbody2D.AddForce (Vector2.right * mMoveForce);
		//ここで動いたり止まったりを調整する
		mStopTime = 0.8f;
	}

	public void Stop(){
		//ストップ中でなければストップ開始
		if(mStopTime <= 0){
			mStopTime = Random.Range(1.5f,2.0f);
		}
	}

	public void DirectionUp () {

	}

	public void DirectionDown () {

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
		return FLIGHT_DURATION;
	}
}
