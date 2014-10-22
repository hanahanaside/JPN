using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainSceneManager : MonoSingleton<MainSceneManager> {
	
	public GameObject stagePrefab;
	public UIGrid stageGrid;
	private UICenterOnChild mCenterOnChild;

	void Start () {
		mCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
		for (int i = 0; i < 9; i++) {
			AddStage ();
		}
		MoveToStage (1);
	}

	public void OnScoutClicked(){
		MoveToStage (0);
	}

	private void MoveToStage (int index) {
		mCenterOnChild.CenterOn (stageGrid.GetChild(index));
	}

	private void AddStage(){
		GameObject stageObject = Instantiate (stagePrefab) as GameObject;
		StageData stageData = new StageData ();
		StageInitializer stageInitializer = stageObject.GetComponentInChildren<StageInitializer> ();
		stageInitializer.InitStage (stageData);
		stageGrid.AddChild (stageObject.transform);
		stageObject.transform.localScale = new Vector3 (1, 1, 1);
	}
}
