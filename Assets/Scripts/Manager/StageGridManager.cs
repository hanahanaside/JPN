using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGridManager : MonoSingleton<StageGridManager> {

	public UIGrid stageGrid;
	public GameObject stagePrefab;
	private UICenterOnChild mCenterOnChild;

	public override void OnInitialize () {
		mCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
	}

	public List<Transform> StageChildList{
		get {
			return stageGrid.GetChildList ();
		}
	}

	public int StageCount {
		get {
			return stageGrid.GetChildList ().Count;
		}
	}

	public  void CreateStageGrid () {
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		foreach (Stage stage in stageList) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			stageGrid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
			stageObject.GetComponentInChildren<StageManager> ().Init (stage);
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
		List<Transform> stageChildList = StageChildList; 
		foreach (Transform stageChild in stageChildList) {
			StageManager stageManager = stageChild.GetComponent<StageManager> ();
			if(stageManager != null){
				stageManager.Resume ();
			}
		}
	}

	public void GenerateLostIdle (int idleId, int count) {
		for (int i = 0; i < count; i++) {
			int stageIndex = CreateStageIndex (idleId);
//			StageManager stageManager = mStageManagerList [stageIndex];
//			stageManager.GenerateLostIdle (idleId);
		}
	}

	public void RemoveIdle(int stageId,int count){
//		StageManager stageManager = mStageManagerList[stageId-1];
//		stageManager.RemoveIdle (count);
	}

	public void GenerateIdle (int idleID) {
//		StageManager stageManager = mStageManagerList [idleID - 1];
//		stageManager.AddIdle (idleID);
	}

	public int GetMaxGeneratePower(){
		int maxGeneratePower = 0;

		return maxGeneratePower;
	}

	private int CreateStageIndex (int idleId) {
		int rand = Random.Range (0, StageGridManager.instance.StageCount);
		while (rand + 1 == idleId) {
			rand = Random.Range (0, StageGridManager.instance.StageCount);
		}
		return rand;
	}
}
