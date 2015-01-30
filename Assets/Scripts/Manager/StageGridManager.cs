using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGridManager : MonoSingleton<StageGridManager> {

	public UIGrid stageGrid;
	public GameObject stagePrefab;
	private UICenterOnChild mCenterOnChild;
	private List<StageManager> mIdolStageManagerList;

	public override void OnInitialize () {
		mCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
	}

	void OnCenterCallBack (GameObject centeredObject) {
		for (int i = 0; i < mIdolStageManagerList.Count; i++) {
			StageManager stageManager = mIdolStageManagerList [i];
			if (centeredObject == stageManager.gameObject) {
				stageManager.IntoFrame ();
				SetActiveCharactor (i);
				SetInActiveCharactor (i);
				break;
			} else {
				stageManager.OutOfFrame ();
			}
		}
	}
		
	//現在の位置から左右のキャラクターをアクティブにする
	private void SetActiveCharactor (int currentIndex) {
		//左をアクティブにする
		if (currentIndex > 0) {
			mIdolStageManagerList [currentIndex - 1].IntoFrame ();
		}
		//右をアクティブにする
		if (currentIndex < mIdolStageManagerList.Count - 1) {
			mIdolStageManagerList [currentIndex + 1].IntoFrame ();
		}
	}

	//現在の位置からアクティブにしたキャラクター以外を非アクティブにする
	private void SetInActiveCharactor (int currentIndex) {
		currentIndex += 2;
		if (currentIndex >= mIdolStageManagerList.Count) {
			return;
		}
		for (int i = currentIndex; i < mIdolStageManagerList.Count; i++) {
			StageManager stageManager = mIdolStageManagerList [i];
			stageManager.OutOfFrame ();
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

	public  void CreateStageGrid () {
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		mIdolStageManagerList = new List<StageManager> ();
		foreach (Stage stage in stageList) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			stageGrid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
			StageManager stageManager = stageObject.GetComponentInChildren<StageManager> ();
			mIdolStageManagerList.Add (stageManager);
			stageManager.Init (stage);
		}
		mCenterOnChild.onCenter += OnCenterCallBack;
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
