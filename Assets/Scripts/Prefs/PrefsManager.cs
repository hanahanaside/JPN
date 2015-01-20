using UnityEngine;
using System.Collections;

public class PrefsManager :Singleton<PrefsManager> {

	public enum Kies {
		PlayerData,
		DatabaseVersion,
		ClearedPuzzleCountArray,
		BGM_ON,
		SE_ON,
		NotificationON,
		LostIdleEvent,
		TradeIdleEvent,
		NewsEvent,
		TutorialFinished,
		LiveData,
		AnnouncedUnlockAreaCount,
		IsReviewed,
		ResumeCount
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

	public int AnnouncedUnlockAreaCount {
		get {
			return PlayerPrefs.GetInt (Kies.AnnouncedUnlockAreaCount.ToString (), 1);
		}
		set {
			PlayerPrefs.SetInt (Kies.AnnouncedUnlockAreaCount.ToString (), value);
			PlayerPrefs.Save ();
		}
	}

	public int ResumeCount{
		get{
			return PlayerPrefs.GetInt (Kies.ResumeCount.ToString(), 0);
		}
		set{
			PlayerPrefs.SetInt (Kies.ResumeCount.ToString(), value);
			PlayerPrefs.Save ();
		}
	}

	public int[] ClearedPuzzleCountArray {
		get {
			int[] clearedPuzzleCountArray = PlayerPrefsX.GetIntArray (Kies.ClearedPuzzleCountArray.ToString (), -2, 8);
			if (clearedPuzzleCountArray [0] == -2) {
				clearedPuzzleCountArray [0] = 1;
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

	public bool TutorialFinished {
		get {
			return PlayerPrefsX.GetBool (Kies.TutorialFinished.ToString (), false);
		}
		set {
			PlayerPrefsX.SetBool (Kies.TutorialFinished.ToString (), value);
		}
	}

	public bool IsReviewed {
		get {
			return PlayerPrefsX.GetBool (Kies.IsReviewed.ToString (), false);
		}
		set {
			PlayerPrefsX.SetBool (Kies.IsReviewed.ToString (), value);
		}
	}

	public void WriteData <T> (T data, Kies key)where T:class {
		string json = JsonFx.Json.JsonWriter.Serialize (data);
		PlayerPrefs.SetString (key.ToString (), json);
		PlayerPrefs.Save ();
	}

	public T Read<T> (Kies key)where T:class, new() {
		string json = PlayerPrefs.GetString (key.ToString ());
		T data = JsonFx.Json.JsonReader.Deserialize<T> (json);
		if (data == null) {
			data = new T ();
		}
		return data;
	}
		
}
