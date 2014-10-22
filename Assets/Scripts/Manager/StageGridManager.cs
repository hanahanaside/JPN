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
		for (int i = 0; i < 47; i++) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			stageGrid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

	public void MoveToStage (int stageIndex) {
		mCenterOnChild.CenterOn (stageGrid.GetChild (stageIndex));
	}

	public int GetIndexNumber(Transform trans){
		return stageGrid.GetIndex (trans);
	}

}
