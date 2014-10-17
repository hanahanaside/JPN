using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainManager : MonoBehaviour {

	public GameObject[] firstAddChildArray;
	public GameObject stagePrefab;
	public UIGrid grid;
	public UICenterOnChild centerOnChild;
	
	// Use this for initialization
	void Start () {
		foreach (GameObject childPrefab in firstAddChildArray) {
			GameObject childObject = Instantiate (childPrefab) as GameObject;
			AddChild (childObject);
		}
		for (int i = 0; i<9; i++) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			StageData stageData = new StageData ();
			StageInitializer stageInitializer = stageObject.GetComponentInChildren<StageInitializer> ();
			stageInitializer.InitStage (stageData);
			AddChild (stageObject);
		}
		centerOnChild.CenterOn(grid.GetChild(1));
	}

	private void AddChild (GameObject panelObject) {
		grid.AddChild (panelObject.transform);
		panelObject.transform.localScale = new Vector3 (1, 1, 1);
	}

}
