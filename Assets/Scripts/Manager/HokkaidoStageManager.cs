﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HokkaidoStageManager : IdleStageManager<HokkaidoStageManager> {

	private List<Idle> idleList;
	private float mUntilSleepTime;

	void Start () {
		mUntilSleepTime = UntilSleepTime;
		idleList = new List<Idle> ();
		for (int i = 0; i < 5; i++) {
			Idle idle = Instantiate (idlePrefab) as Idle;
			idle.transform.parent = transform.parent;
			idle.transform.localScale = new Vector3 (1f, 1f, 1f);
			float x = Random.Range (idle.movableArea.limitLeft, idle.movableArea.limitRight);
			float y = Random.Range (idle.movableArea.limitBottom, idle.movableArea.limitTop);
			idle.transform.localPosition = new Vector3 (x, y, 0);
			idleList.Add (idle);
		}
	}

	void Update () {
		if (IsSleeping) {
			return;
		}
		mUntilSleepTime -= Time.deltaTime;
		UntilSleepLabel  = "あと" + TimeConverter.Convert (mUntilSleepTime) + "でサボる";
		if (mUntilSleepTime > 0) {
			return;
		}
		foreach (Idle idle in idleList) {
			idle.Sleep ();
		}
		IsSleeping = true;
		IdleSprite = "idle_sleep_1";
		ShowFence ();
		ShowWakeupButton ();
	}

	public override void OnWakeupButtonClicked(){
		IsSleeping = false;
		HideFence ();
		HideWakeupButton ();
		foreach(Idle idle in idleList){
			idle.Wakeup ();
		}
		mUntilSleepTime = UntilSleepTime;
	}

	public override void StartLive () {
		if(IsSleeping){
			OnWakeupButtonClicked ();
		}
		foreach (Idle idle in idleList) {
			idle.GetComponentInChildren<Idle> ().StartDancing ();
		}
	}
		
}
