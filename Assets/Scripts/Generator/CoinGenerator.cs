using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoBehaviour {

	public GameObject coinPrefab;
	public UICenterOnChild uiCenterOnChild;
	private GameObject mCenterObject;
	private float mInterval = 5.0f;

	void Update () {
		mCenterObject = uiCenterOnChild.centeredObject;
		mInterval -= Time.deltaTime;
		if (mInterval < 0) {
			GameObject coinObject = Instantiate (coinPrefab) as GameObject;
			coinObject.transform.parent = mCenterObject.transform;
			coinObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			mInterval = 5.0f;
		}
	}

}
