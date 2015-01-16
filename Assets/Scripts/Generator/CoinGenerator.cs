using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinGenerator : MonoSingleton<CoinGenerator> {

	public GameObject[] coinPrefabArray;
	public UICenterOnChild uiCenterOnChild; 
	public UIGrid stageGrid;
	private GameObject mCenteredObject;
	private float mInterval = 5.0f;
	private bool mStop = false;

	void Awake () {
		uiCenterOnChild.onCenter += OnCenterCallBack;
	}

	void Update () {
		if (mStop) {
			return;
		}
		mInterval -= Time.deltaTime;
		if (mInterval > 0) {
			return;
		}
		int rand = UnityEngine.Random.Range (0,StageGridManager.instance.StageManagerList.Count);
		rand = 1;
		mCenteredObject = StageGridManager.instance.StageManagerList [rand].gameObject;

		if (mCenteredObject.tag == "sleep" || mCenteredObject.tag == "construction") {
			mInterval = 5.0f;
			return;
		}
		GameObject coinPrefab = GetCoinPrefab ();
		GameObject coinObject = Instantiate (coinPrefab) as GameObject;
		coinObject.transform.parent = mCenteredObject.transform;
		coinObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		mInterval = 5.0f;
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GenerateCoin);
	}

	void OnCenterCallBack (GameObject centeredObject) {
	//	mCenteredObject = centeredObject;
	}

	public void StopGenerating () {
		mStop = true;
	}

	public void StartGenerating () {
		mStop = false;
	}

	private GameObject GetCoinPrefab () {
		int[] clearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		int unlockStageCount = 0;
		for (int i = 0; i < clearedPuzzleCountArray.Length; i++) {
			int clearedCount = clearedPuzzleCountArray [i];
			if (clearedCount < 0) {
				unlockStageCount = i;
				break;
			}
			unlockStageCount = i;
		}
		int coinIndex = 0;
		switch (unlockStageCount) {
		case 1:
		case 2:
			coinIndex = CoinRate.GetCoinIndexLevel_1 ();
			break;
		case 3:
		case 4:
			coinIndex = CoinRate.GetCoinIndexLevel_2 ();
			break;
		case 5:
		case 6:
			coinIndex = CoinRate.GetCoinIndexLevel_3 ();
			break;
		case 7:
		case 8:
			coinIndex = CoinRate.GetCoinIndexLevel_4 ();
			break;
		}
		return coinPrefabArray [coinIndex];
	}

}
