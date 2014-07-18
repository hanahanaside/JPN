using UnityEngine;
using System.Collections;

public class MainInitializer : MonoBehaviour {

	public GameObject homePrefab;
	public GameObject stagePrefab; 
	public UIGrid grid;
	
	// Use this for initialization
	void Start () {
		GameObject homeObject = Instantiate (homePrefab) as GameObject;
		AddChild (homeObject);
		for (int i = 0; i<9; i++) {
			GameObject stageObject = Instantiate (stagePrefab) as GameObject;
			AddChild (stageObject);
		}
	}

	private void AddChild (GameObject panelObject) {
		grid.AddChild (panelObject.transform);
		panelObject.transform.localScale = new Vector3 (1, 1, 1);
	}

}
