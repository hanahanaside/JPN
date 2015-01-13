using UnityEngine;
using System.Collections;

public static class CoinRate {

	public static int GetCoinIndexLevel_1 () { 
		int rand = UnityEngine.Random.Range (0, 100);
		if (rand < 50) {
			return 0;
		}
		return 1;
	}

	public static int GetCoinIndexLevel_2 () {
		int rand = UnityEngine.Random.Range (0, 100);
		if (rand < 30) {
			return 0;
		}
		if (rand < 70) {
			return 1;
		}
		return 2;
	}

	public static int GetCoinIndexLevel_3 () {
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

	public static int GetCoinIndexLevel_4 () {
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
