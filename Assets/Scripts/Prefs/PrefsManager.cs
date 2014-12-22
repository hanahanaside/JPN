using UnityEngine;
using System.Collections;

public class PrefsManager :Singleton<PrefsManager> {

	private enum Kies {
		PlayerData,
		DatabaseVersion,
		ClearedPuzzleCountArray,
		BGM_ON,
		SE_ON,
		NotificationON
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
			int[] clearedPuzzleCountArray = PlayerPrefsX.GetIntArray (Kies.ClearedPuzzleCountArray.ToString (), -2, 8);
			if (clearedPuzzleCountArray [0] == -2) {
				clearedPuzzleCountArray [0] = 0;
				clearedPuzzleCountArray [1] = -1;
				ClearedPuzzleCountArray = clearedPuzzleCountArray;
			}
			return clearedPuzzleCountArray;
		}
		set {
			PlayerPrefsX.SetIntArray (Kies.ClearedPuzzleCountArray.ToString (), value);
		}
	}

	public bool BGM_ON {
		get {
			return PlayerPrefsX.GetBool (Kies.BGM_ON.ToString (), true);
		}
		set {
			PlayerPrefsX.SetBool (Kies.BGM_ON.ToString (), value);
		}
	}

	public bool SE_ON {
		get {
			return PlayerPrefsX.GetBool (Kies.SE_ON.ToString (), true);
		}
		set {
			PlayerPrefsX.SetBool (Kies.SE_ON.ToString (), value);
		}
	}

	public bool NotificationON {
		get {
			return PlayerPrefsX.GetBool (Kies.NotificationON.ToString (), true);
		}
		set {
			PlayerPrefsX.SetBool (Kies.NotificationON.ToString (), value);
		}
	}
}
