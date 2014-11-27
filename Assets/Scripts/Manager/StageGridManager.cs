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
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		mStageManagerList = new List<StageManager> ();
		foreach(Stage stage in stageList){
			GameObject stagePrefab = Resources.Load ("Stage/Stage_" + stage.Id) as GameObject;
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
