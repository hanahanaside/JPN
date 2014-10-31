using UnityEngine;
using System.Collections;

public class IdleDanceState : IdleState {

	private const float INTERVAL_TIME = 0.5f;
	private float mJumpForce = 1.0f;
	private float mMoveForce = 5.0f;

	public void Move (GameObject gameobject) {
		Rigidbody2D rigidbody2D = gameobject.rigidbody2D;
		rigidbody2D.velocity = Vector2.up * mJumpForce;
		rigidbody2D.AddForce (Vector2.right * mMoveForce);
	}

	public void Stop(){

	}

	public void DirectionUp(){

	}

	public void DirectionDown(){

	}

	public void DirectionLeft(){
		if(mMoveForce > 0){
			mMoveForce = -mMoveForce;
		}
	}

	public void DirectionRight(){
		if(mMoveForce < 0){
			mMoveForce = -mMoveForce;
		}
	}

	public float FlightDuration(){
		return INTERVAL_TIME;
	}
}
