using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoSingleton<ObjectPool> {

	private const int MAX_STAGE_COUNT = 48;

	public GameObject idolStagePrefab;

	private GameObject[] mStageObjectArray;

	public override	void OnInitialize () {
		mStageObjectArray = new GameObject[MAX_STAGE_COUNT];
		DontDestroyOnLoad (gameObject);
	}

	public void CreateObjects () {
		StageDao dao = DaoFactory.CreateStageDao ();
		List<StageData> stageManagerList = dao.SelectAll ();
		for (int i = 0; i < stageManagerList.Count; i++) {
			GameObject stageObject = Instantiate (idolStagePrefab) as GameObject;
			DontDestroyOnLoad (stageObject);
			stageObject.transform.parent = transform;
			stageObject.SetActive (false);
			mStageObjectArray [i] = stageObject;
		}
	}

	public GameObject GetStageObject (int index) {
		GameObject stageObject = mStageObjectArray [index];
		if (stageObject == null) {
			Debug.Log ("null");
			stageObject = Instantiate (idolStagePrefab) as GameObject;
			mStageObjectArray [index] = stageObject;
		}
		stageObject.SetActive (true);
		return stageObject;
	}
}
