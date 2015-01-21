using UnityEngine;
using System.Collections;

public class TradeIdleEvent {

	public int idleID;
	public int idleCount;
	public int reward;
	public bool occurring;
	private string lastUpdateDate;

	public string LastUpdateDate{
		get{
			if(string.IsNullOrEmpty(lastUpdateDate)){
				lastUpdateDate = System.DateTime.Now.ToString ();
				PrefsManager.instance.WriteData<TradeIdleEvent> (this, PrefsManager.Kies.TradeIdleEvent);
			}
			return lastUpdateDate;
		}
		set{
			lastUpdateDate = value;
		}
	}
}
