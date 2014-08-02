using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoBehaviour {

	public GameObject coinPanelPrefab;
	public UICenterOnChild uiCenterOnChild;
	private GameObject mCenterObject;

	IEnumerator Start () {
		yield return new WaitForSeconds (5f);
		GameObject coinPanelObject = Instantiate (coinPanelPrefab) as GameObject;
		coinPanelObject.transform.parent = mCenterObject.transform;
		coinPanelObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		StartCoroutine (Start ());
	}

	void Update () {
		mCenterObject = uiCenterOnChild.centeredObject;
	}

}
