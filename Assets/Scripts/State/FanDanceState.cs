using UnityEngine;
using System.Collections;

public class FanDanceState : FanState {

	private Fan mFan;
	private float mTime;

	public FanDanceState(Fan fan){
		mFan = fan;
		mTime = Random.Range (1.0f,3.0f);
	}

	public void Move () {
		mTime -= Time.deltaTime;
		if(mTime < 0){
			mFan.ChangeDirection ();
			mTime = Random.Range (1.0f,3.0f);
		}
		mFan.CheckFlip ();
		mFan.Move ();
	}
}
