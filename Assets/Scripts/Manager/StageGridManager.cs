using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGridManager : MonoSingleton<StageGridManager> {

	public UIGrid stageGrid;
	private UICenterOnChild mCenterOnChild;

	public override void OnInitialize () {
		mCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
	}

	public  void CreateStageGrid () {
		for (int i = 0; i < 47; i++) {
			GameObject stagePrefab = Resources.Load ("Stage/Stage_1") as GameObject;
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			stageGrid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

	public void MoveToStage (int stageIndex) {
		mCenterOnChild.CenterOn (stageGrid.GetChild (stageIndex));
	}
		
	public void StartLive(){
		HokkaidoStageManager.instance.StartLive ();
	}
}
