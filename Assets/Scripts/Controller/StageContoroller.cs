using UnityEngine;
using System.Collections;

public class StageContoroller : MonoBehaviour {

	public StageDataKeeper stageDataKeeper;

	void OnApplicationPause (bool pauseState) {
		if (pauseState) {
			stageDataKeeper.SaveStageData ();
		}
	}

}
