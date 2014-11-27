using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGridManager : MonoSingleton<StageGridManager> {

	public UIGrid stageGrid;
	private UICenterOnChild mCenterOnChild;
	private List<StageManager> mStageManagerList;

	public override void OnInitialize () {
		mCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
	}

	public List<StageManager> StageManagerList {
		get {
			return mStageManagerList;
		}
	}

	public  void CreateStageGrid () {
		mStageManagerList = new List<StageManager> ();
		int count = StageListKeeper.instance.ListCount;
		for (int i = 0; i < count; i++) {
			Stage stageData = StageListKeeper.instance.GetStageData (i);
			GameObject stagePrefab = Resources.Load ("Stage/Stage_" + stageData.Id) as GameObject;
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			stageGrid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
			mStageManagerList.Add (stageObject.GetComponentInChildren<StageManager> ());
		}
	}

	public void MoveToStage (int stageIndex) {
		mCenterOnChild.CenterOn (stageGrid.GetChild (stageIndex));
	}

}
