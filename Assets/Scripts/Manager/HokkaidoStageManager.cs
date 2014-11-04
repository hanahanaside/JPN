using UnityEngine;
using System.Collections;

public class HokkaidoStageManager : IdleStageManager<HokkaidoStageManager> {

	void Start () {
		Debug.Log ("start");
		for(int i = 0; i<5;i++){
			GameObject idle =  Instantiate (idlePrefab) as GameObject;
			idle.transform.parent = transform.parent;
			idle.transform.localScale = new Vector3 (1f,1f,1f);
			idle.transform.localPosition = new Vector3 (0,0,0);
		}

	}
}
