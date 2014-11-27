using UnityEngine;
using System.Collections;

public class PrefsManager :Singleton<PrefsManager> {

	private enum Kies {
		PlayerData
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


}
