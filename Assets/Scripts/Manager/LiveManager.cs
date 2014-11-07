using UnityEngine;
using System.Collections;

public class LiveManager : MonoSingleton<LiveManager> {

	public GameObject livePanelObject;

	private float mTime;
	private bool mLive;

	// Update is called once per frame
	void Update () {
		if(!mLive){
			return;
		}	
		mTime -= Time.deltaTime;
		if(mTime > 0){
			return;
		}
		mLive = false;
		livePanelObject.SetActive (false);
	}

	public void StartLive(){
		mTime = 10.0f;
		livePanelObject.SetActive (true);
		mLive = true;
	}
}
