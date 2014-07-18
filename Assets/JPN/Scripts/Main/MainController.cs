using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

	public GameObject prefab;
	public UIGrid grid;

	// Use this for initialization
	void Start () {
		for(int i = 0; i<9;i++) {
			GameObject stageObject = Instantiate (prefab) as GameObject;
			grid.AddChild (stageObject.transform);
			stageObject.transform.localScale = new Vector3 (1,1,1);

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
