using UnityEngine;
using System.Collections;

public class PrefsManager :Singleton<PrefsManager> {

	public void SavePlayerData(string playerData){
		PlayerPrefs.SetString (PlayerPrefsKies.PLAYER_DATA,playerData);
		PlayerPrefs.Save ();
	}

	public string LoadPlayerData(){
		return PlayerPrefs.GetString (PlayerPrefsKies.PLAYER_DATA);
	}

	private class PlayerPrefsKies{
		public const string PLAYER_DATA  = "playerData";
	}
}
