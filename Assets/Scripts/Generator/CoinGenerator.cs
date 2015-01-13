using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoSingleton<CoinGenerator> {

	public GameObject[] coinPrefabArray;
	public UICenterOnChild uiCenterOnChild;
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
		mCenteredObject = centeredObject;
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
			coinIndex = GetCoinIndexLevel_1 ();
			break;
		case 3:
		case 4:
			coinIndex = GetCoinIndexLevel_2();
			break;
		case 5:
		case 6:
			coinIndex = GetCoinIndexLevel_3();
			break;
		case 7:
		case 8:
			coinIndex = GetCoinIndexLevel_4();
			break;
		}
		return coinPrefabArray [coinIndex];
	}

	private int GetCoinIndexLevel_1 () { 
		int rand = UnityEngine.Random.Range (0, 100);
		if (rand < 50) {
			return 0;
		}
		return 1;
	}

	private int GetCoinIndexLevel_2 () {
		int rand = UnityEngine.Random.Range (0, 100);
		if (rand < 30) {
			return 0;
		}
		if (rand < 70) {
			return 1;
		}
		return 2;
	}

	private int GetCoinIndexLevel_3 () {
		int rand = UnityEngine.Random.Range (0, 100);
		if (rand < 25) {
			return 0;
		}
		if (rand < 50) {
			return 1;
		}
		if (rand < 75) {
			return 2;
		}
		return 3;
	}

	private int GetCoinIndexLevel_4 () {
		int rand = UnityEngine.Random.Range (0, 100);
		if (rand < 25) {
			return 0;
		}
		if (rand < 50) {
			return 1;
		}
		if (rand < 75) {
			return 2;
		}
		if (rand < 95) {
			return 3;
		}
		return 4;
	}
}
