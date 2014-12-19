﻿using UnityEngine;
using System.Collections;

public class PrefsManager :Singleton<PrefsManager> {

	private enum Kies {
		PlayerData,
		DatabaseVersion,
		ClearedPuzzleCountArray
	} 

	public string PlayerDataJson {
		get {
			return PlayerPrefs.GetString (Kies.PlayerData.ToString ());
		}
		set {
			PlayerPrefs.SetString (Kies.PlayerData.ToString (), value);
			PlayerPrefs.Save ();
		}
	}

	public int DatabaseVersion {
		get {
			return PlayerPrefs.GetInt (Kies.DatabaseVersion.ToString (), 0);
		}
		set {
			PlayerPrefs.SetInt (Kies.DatabaseVersion.ToString (), value);
			PlayerPrefs.Save ();
		}
	}

	public int[] ClearedPuzzleCountArray {
		get {
			int[] clearedPuzzleCountArray = PlayerPrefsX.GetIntArray (Kies.ClearedPuzzleCountArray.ToString(), -2, 8);
			if(clearedPuzzleCountArray[0] == -2){
				clearedPuzzleCountArray [0] = 0;
				clearedPuzzleCountArray [1] = -1;
				ClearedPuzzleCountArray = clearedPuzzleCountArray;
			}
			return clearedPuzzleCountArray;
		}
		set {
			PlayerPrefsX.SetIntArray (Kies.ClearedPuzzleCountArray.ToString(), value);
		}
	}
}