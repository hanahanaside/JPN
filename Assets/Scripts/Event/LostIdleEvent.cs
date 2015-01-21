using UnityEngine;
using System.Collections;

public class LostIdleEvent {

	public int lostIdleID;
	public int lostIdleCount;
	public int foundIdleCount;
	public int reward;
	public bool occurring;
	private string lastUpdateDate;

	public string LastUpdateDate{
		get{
			if(string.IsNullOrEmpty(lastUpdateDate)){
				lastUpdateDate = System.DateTime.Now.ToString ();
				PrefsManager.instance.WriteData<LostIdleEvent> (this, PrefsManager.Kies.LostIdleEvent);
			}
			return lastUpdateDate;
		}
		set{
			lastUpdateDate = value;
		}
	}
}
