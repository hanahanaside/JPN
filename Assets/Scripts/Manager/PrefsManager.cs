using UnityEngine;
using System.Collections;

public class PrefsManager :Singleton<PrefsManager> {

	private enum Kies {
		PlayerData,
		DatabaseVersion
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

	public int DatabaseVersion{
		get{
			return PlayerPrefs.GetInt (Kies.DatabaseVersion.ToString(),0);
		}
		set{
			PlayerPrefs.SetInt (Kies.DatabaseVersion.ToString(),value);
			PlayerPrefs.Save ();
		}
	}
}
