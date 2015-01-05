﻿using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoSingleton<CoinGenerator> {

	public GameObject[] coinPrefabArray;
	public UICenterOnChild uiCenterOnChild;
	private GameObject mCenteredObject;
	private float mInterval = 5.0f;
	private bool mStop = false;

	void Awake () {
		uiCenterOnChild.onCenter = OnCenterCallBack;
	}

	void Update () {
		if (mStop) {
			return;
		}
		mInterval -= Time.deltaTime;
		if (mInterval > 0) {
			return;
		}
		if (mCenteredObject.tag == "sleep") {
			mInterval = 5.0f;
			return;
		}
		int[] numbers = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4 };
		int rand = Random.Range (0, numbers.Length);

		GameObject coinPrefab = coinPrefabArray [numbers [rand]];
		GameObject coinObject = Instantiate (coinPrefab) as GameObject;
		coinObject.transform.parent = mCenteredObject.transform;
		coinObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		mInterval = 5.0f;
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GenerateCoin);
	}

	void OnCenterCallBack (GameObject centeredObject) {
		mCenteredObject = centeredObject;
	}

	public void StopGenerating () {
		mStop = true;
	}

	public void StartGenerating () {
		mStop = false;
	}
}
