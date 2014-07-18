using UnityEngine;
using System.Collections;

public class StageContoroller : MonoBehaviour {

	public GameObject idlePrefab;
	public GameObject fanPrefab;

	// Use this for initialization
	void Start () {
		GenerateIdle ();
		GenerateFan ();
	}
	
	private void GenerateIdle () {
		for (int i = 0; i<5; i++) {
			float x = Random.Range (-200.0f, 200.0f);
			float y = Random.Range (-50.0f, 300.0f);
			GameObject idleObject = Instantiate (idlePrefab) as GameObject;
			idleObject.transform.parent = transform.parent;
			idleObject.transform.localScale = new Vector3 (1, 1, 1);
			idleObject.transform.localPosition = new Vector3 (x, y, 0);
		}
	}

	private void GenerateFan () {
		for (int i = 0; i<5; i++) {
			float x = Random.Range (-200.0f, 200.0f);
			GameObject fanObject = Instantiate (fanPrefab)as GameObject;
			fanObject.transform.parent = transform.parent;
			fanObject.transform.localScale = new Vector3 (1, 1, 1);
			fanObject.transform.localPosition = new Vector3 (x, -200, 0);
		}
	}
}
