using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntranceStageManager : MonoSingleton<EntranceStageManager>{

	public GameObject startLiveButton;
	private List<Fan> mFanList = new List<Fan> ();

	void Awake () {
		for (int i = 0; i < 30; i++) {
			int rand = UnityEngine.Random.Range (1, 14);
			GameObject fanPrefab = Resources.Load ("Model/Fan/Fan_" + rand) as GameObject;
			GameObject fanObject = Instantiate (fanPrefab) as GameObject;
			float x = UnityEngine.Random.Range (-200.0f, 200.0f);
			float y = UnityEngine.Random.Range (-210.0f, 70.0f);
			fanObject.transform.parent = transform;
			fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			fanObject.transform.localPosition = new Vector3 (x, y, 0);
			Fan fan = fanObject.GetComponent<Fan> ();
			fan.movableArea.limitTop = 70.0f;
			fan.movableArea.limitBottom = -210.0f;
			fan.movableArea.limitLeft = -200.0f;
			fan.movableArea.limitRight = 200.0f;
			mFanList.Add (fan);
			fanObject.GetComponent<Fan> ().Init ();
		}
	}

	public void OnLiveButtonClicked(){
		FenceManager.instance.ShowFence ();
		SelectLiveTimeDialogManager.instance.Show ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void StartLive(){
		startLiveButton.SetActive (false);
		foreach(Fan fan in mFanList){
			fan.StartLive ();
		}
	}

	public void FinishLive(){
		startLiveButton.SetActive (true);
		foreach(Fan fan in mFanList){
			fan.FinishLive ();
		}
	}
}
