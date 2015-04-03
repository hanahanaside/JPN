using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinGenerator : MonoSingleton<CoinGenerator> {

	public GameObject[] coinPrefabArray;
	public UIGrid stageGrid;
	private float interval = 5.0f;
	private float mUntilGenerateTimeSeconds;
	private bool mStop = false;
	private int mUnlockStageCount;
	private GameObject mCenteredObject;

	void Awake () {
		mUntilGenerateTimeSeconds = interval;
		mCenteredObject = stageGrid.GetChildList () [0].gameObject;
		stageGrid.GetComponent<UICenterOnChild> ().onCenter += OnCenterCallBack;
		int[] clearedPuzzleCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		for (int i = 0; i < clearedPuzzleCountArray.Length; i++) {
			int clearedCount = clearedPuzzleCountArray [i];
			if (clearedCount < 0) {
				mUnlockStageCount = i;
				break;
			}
			mUnlockStageCount = clearedPuzzleCountArray.Length;
		}
	}

	void Update () {
		if (mStop) {
			return;
		}
		mUntilGenerateTimeSeconds -= Time.deltaTime;
		if (mUntilGenerateTimeSeconds > 0) {
			return;
		}
		int rand = UnityEngine.Random.Range (0, stageGrid.GetChildList ().Count);
		GameObject stageObject = stageGrid.GetChildList () [rand].gameObject;

		if (stageObject.tag == "sleep" || stageObject.tag == "construction") {
			mUntilGenerateTimeSeconds = interval;
			return;
		}
			
		GameObject coinPrefab = GetCoinPrefab ();
		NGUITools.AddChild (mCenteredObject, coinPrefab);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GenerateCoin);
		mUntilGenerateTimeSeconds = interval;
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

	public void StartLive () {
		interval = 2.5f;
	}

	public void FinishLive () {
		interval = 5.0f;
	}

	private GameObject GetCoinPrefab () {
		int coinIndex = 0;
		switch (mUnlockStageCount) {
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
		Debug.Log (mUnlockStageCount);
		return coinPrefabArray [coinIndex];
	}

}
