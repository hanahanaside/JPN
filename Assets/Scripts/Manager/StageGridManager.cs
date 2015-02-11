using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGridManager : MonoSingleton<StageGridManager> {

	public UIGrid stageGrid;
	public GameObject stagePrefab;
	private UICenterOnChild mCenterOnChild;
	private List<StageManager> mIdolStageManagerList;
	private GameObject mCenteredObject;

	public override void OnInitialize () {
		mCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
		mCenterOnChild.onCenter += OnCenter;
	}

	void OnCenter (GameObject centeredObject) {
		mCenteredObject = centeredObject;
		if (centeredObject.tag == "sleep") {
			AdManager.instance.ShowIconAd ();
		} else {
			AdManager.instance.HideIconAd ();
		}
	}

	public List<StageManager> StageManagerList {
		get {
			return mIdolStageManagerList;
		}
	}

	public int StageCount {
		get {
			return mIdolStageManagerList.Count;
		}
	}

	public int GetCenterdObjectIndex {
		get {
			int index = 0;
			for (int i = 0; i < mIdolStageManagerList.Count; i++) {
				GameObject idolStageObject = mIdolStageManagerList [i].gameObject;
				if (mCenteredObject == idolStageObject) {
					index = i;
					break;
				}
			}
			return index;
		}
	}

	public  void CreateStageGrid () {
		mIdolStageManagerList = new List<StageManager> ();
		StageDao dao = DaoFactory.CreateStageDao ();
		List<StageData> stageDataList = dao.SelectAll ();
		foreach (StageData stage in stageDataList) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			stageGrid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
			StageManager stageManager = stageObject.GetComponentInChildren<StageManager> ();
			mIdolStageManagerList.Add (stageManager);
			stageManager.Init (stage);
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
		foreach (StageManager stageManager in mIdolStageManagerList) {
			stageManager.Resume ();
		}
	}

	public void GenerateLostIdle (int idleId, int count) {
		for (int i = 0; i < count; i++) {
			int stageIndex = CreateStageIndex (idleId);
			StageManager stageManager = mIdolStageManagerList [stageIndex];
			stageManager.GenerateLostIdle (idleId);
		}
	}

	public void RemoveIdle (int stageId, int count) {
		StageManager stageManager = mIdolStageManagerList [stageId - 1];
		stageManager.RemoveIdle (count);
	}

	public void GenerateIdle (int idleID, int count) {
		StageManager stageManager = mIdolStageManagerList [idleID - 1];
		stageManager.AddIdle (count);
	}

	public int GetMaxGeneratePower () {
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
