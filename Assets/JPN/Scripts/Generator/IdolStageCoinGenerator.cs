using UnityEngine;
using System.Collections;

public class IdolStageCoinGenerator : MonoBehaviour {

	private const float UNTIL_GENERATE_TIME = 0.6f;
	private float mUntilGenerateTime = UNTIL_GENERATE_TIME;

	public void PassTime(float time,double generateCoinPower){
		mUntilGenerateTime -= time;
		if (mUntilGenerateTime < 0) {
			PlayerDataKeeper.instance.IncreaseCoinCount (generateCoinPower / 100.0);
			mUntilGenerateTime = UNTIL_GENERATE_TIME;
		}
	}
}
