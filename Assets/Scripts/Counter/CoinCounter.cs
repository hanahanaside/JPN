using UnityEngine;
using System.Collections;

public class CoinCounter : MonoBehaviour {

	private const float INTERVAL_TIME = 0.6f;
	private float mIntervalTime = INTERVAL_TIME;

	void Update () {
		mIntervalTime -= Time.deltaTime;
		if (mIntervalTime < 0) {
			double addCoinCount = PlayerDataKeeper.instance.GenerateCoinSpeed / 100.0;
			PlayerDataKeeper.instance.UpdateCoinCount (addCoinCount);
			mIntervalTime = INTERVAL_TIME;
		}
	}
}
