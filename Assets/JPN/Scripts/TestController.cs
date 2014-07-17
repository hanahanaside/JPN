using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public GameObject stagePrefab;
	public GameObject scrollView;
	public UIGrid uiGrid;

	// Use this for initialization
	void Start () {
		for(int i = 0;i <4;i++){
			GameObject stageObject = Instantiate(stagePrefab)as GameObject;
			uiGrid.AddChild(stageObject.transform);
			stageObject.transform.localScale = new Vector3(1,1,1);
			UISprite sprite = stageObject.GetComponentInChildren<UISprite>();
			sprite.spriteName = "bg_main_"+i;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
