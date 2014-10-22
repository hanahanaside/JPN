using UnityEngine;
using System.Collections;

public class StageGridManager : MonoSingleton<StageGridManager> {

	public GameObject stagePrefab;
	public UIGrid stageGrid;
	private UICenterOnChild mCenterOnChild;

	public override void OnInitialize () {
		mCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
	}

	public  void CreateStageGrid () {
		for (int i = 0; i < 9; i++) {
			AddStage ();
		}
	}

	public void MoveToStage (int stageIndex) {
		mCenterOnChild.CenterOn (stageGrid.GetChild (stageIndex));
	}

	private void AddStage () {
		GameObject stageObject = Instantiate (stagePrefab) as GameObject;
		StageData stageData = new StageData ();
		StageInitializer stageInitializer = stageObject.GetComponentInChildren<StageInitializer> ();
		stageInitializer.InitStage (stageData);
		stageGrid.AddChild (stageObject.transform);
		stageObject.transform.localScale = new Vector3 (1, 1, 1);
	}
}
