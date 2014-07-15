using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public GameObject stagePrefab;
	public GameObject scrollView;

	// Use this for initialization
	void Start () {
		for(int i = 0;i <5;i++){
			GameObject stageObject = Instantiate(stagePrefab)as GameObject;
			stageObject.transform.parent = scrollView.transform;
			stageObject.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
