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

	public int StageCount {
		get {
			return mStageManagerList.Count;
		}
	}

	public  void CreateStageGrid () {
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		mStageManagerList = new List<StageManager> ();
		foreach (Stage stage in stageList) {
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

	public void MoveToSleepStage () {
		List<Transform> childList = stageGrid.GetChildList ();
		for (int i = 0; i < childList.Count; i++) {
			Transform childTransform = childList [i];
			if (childTransform.gameObject.tag == "sleep") {
				MoveToStage (i);
				break;
			}
		}
	}

	public void Resume () {
		foreach (StageManager stageManager in mStageManagerList) {
			stageManager.Resume ();
		}
	}

	public void GenerateLostIdle (int idleId, int count) {
		for (int i = 0; i < count; i++) {
			int stageIndex = CreateStageIndex (idleId);
			StageManager stageManager = mStageManagerList [stageIndex];
			stageManager.GenerateLostIdle (idleId);
		}
	}

	public void RemoveIdle(int stageId,int count){
		StageManager stageManager = mStageManagerList[stageId-1];
		stageManager.RemoveIdle (count);
	}

	public void GenerateIdle () {
		LostIdleEvent lostIdleEvent =  Resources.Load ("Event/LostIdleEvent") as LostIdleEvent;
		StageManager stageManager = mStageManagerList [lostIdleEvent.lostIdleID - 1];
		stageManager.AddIdle (lostIdleEvent.lostIdleCount);
	}

	private int CreateStageIndex (int idleId) {
		int rand = Random.Range (0, StageGridManager.instance.StageCount);
		while (rand + 1 == idleId) {
			rand = Random.Range (0, StageGridManager.instance.StageCount);
		}
		return rand;
	}
}
