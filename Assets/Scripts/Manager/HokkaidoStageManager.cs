using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HokkaidoStageManager : IdleStageManager<HokkaidoStageManager> {

	private List<HokkaidoIdle> idleList;

	void Start () {
		idleList = new List<HokkaidoIdle> ();
		for(int i = 0; i<5;i++){
			HokkaidoIdle idle =  Instantiate (idlePrefab) as HokkaidoIdle;
			idle.transform.parent = transform.parent;
			idle.transform.localScale = new Vector3 (1f,1f,1f);
			float x = Random.Range (idle.movableArea.limitLeft,idle.movableArea.limitRight);
			float y = Random.Range (idle.movableArea.limitBottom,idle.movableArea.limitTop);
			idle.transform.localPosition = new Vector3 (x,y,0);
			idleList.Add (idle);
		}
	}

	public override void StartLive(){
		foreach(HokkaidoIdle idle in idleList){
			idle.GetComponentInChildren<Idle> ().StartDancing ();
		}
	}
}
