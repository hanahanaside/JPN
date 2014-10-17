using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainSceneManager : MonoBehaviour {
	
	public GameObject stagePrefab;
	public UIGrid grid;
	private UICenterOnChild mCenterOnChild;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 9; i++) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			StageData stageData = new StageData ();
			StageInitializer stageInitializer = stageObject.GetComponentInChildren<StageInitializer> ();
			stageInitializer.InitStage (stageData);
			grid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1, 1, 1);
		}
		mCenterOnChild = grid.GetComponent<UICenterOnChild> ();
		mCenterOnChild.CenterOn (grid.GetChild (1));
	}
}
