using UnityEngine;
using System.Collections;

public class EventManager : MonoSingleton<EventManager> {

	public GameObject sleepButtonObject;
	public GameObject liveButtonObject;
	public GameObject LostButtonObject;
	public GameObject TransferButtonObject;
	public GameObject newsButtonObject;

	private int mSleepStageCount;

	void OnEnable () {
		StageManager.SleepEvent += SleepEvent;
		StageManager.WakeupEvent += WakeupEvent;
	}

	void OnDisable () {
		StageManager.SleepEvent -= SleepEvent;
		StageManager.WakeupEvent -= WakeupEvent;
	}

	void SleepEvent () {
		mSleepStageCount++;
		if (!sleepButtonObject.activeSelf) {
			sleepButtonObject.SetActive (true);
			StartScaleEvent (sleepButtonObject);
		}
	}

	void WakeupEvent () {
		mSleepStageCount--;
		if (mSleepStageCount <= 0) {
			StopScaleEvent (sleepButtonObject);
			sleepButtonObject.SetActive (false);
		}
	}

	public void SleepButtonClicked () {
		StageGridManager.instance.MoveToSleepStage ();
	}

	private void StartScaleEvent (GameObject buttonObject) {
		iTweenEvent.GetEvent (buttonObject, "ScaleEvent").Play ();
	}

	private void StopScaleEvent (GameObject buttonObject) {
		iTweenEvent.GetEvent (buttonObject, "ScaleEvent").Stop ();
		buttonObject.transform.localScale = new Vector3 (1, 1, 1);
	}
}
