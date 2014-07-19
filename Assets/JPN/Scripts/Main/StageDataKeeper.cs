using UnityEngine;
using System.Collections;

public class StageDataKeeper : MonoBehaviour {

	public StageTimeKeeper stageTimeKeeper;
	private StageData mStageData;

	public void SetStageData (StageData stageData) {
		mStageData = stageData;
	}

	public void SaveStageData () {
		Debug.Log ("Save StageData");
		int untilSleepSeconds = stageTimeKeeper.UntilSleepSeconds;
		mStageData.UntilSleepSeconds = untilSleepSeconds;
	}
}
