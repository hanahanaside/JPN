using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoBehaviour {

	public GameObject coinPrefab;
	public UICenterOnChild uiCenterOnChild;
	private GameObject mCenteredObject;
	private float mInterval = 5.0f;

	void Awake () {
		uiCenterOnChild.onCenter = OnCenterCallBack;
	}

	void Update () {
		mInterval -= Time.deltaTime;
		if (mInterval < 0) {
			GameObject coinObject = Instantiate (coinPrefab) as GameObject;
			coinObject.transform.parent = mCenteredObject.transform;
			coinObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			mInterval = 5.0f;
		}
	}

	void OnCenterCallBack (GameObject centeredObject) {
		mCenteredObject = centeredObject;
	}
}
