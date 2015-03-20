using UnityEngine;
using System.Collections;

public class NewsEvent {

	public int reward;
	public string message;
	public string unit;
	public bool occurring;
	private string lastUpdateDate;

	public string LastUpdateDate{
		get{
			if(string.IsNullOrEmpty(lastUpdateDate)){
				lastUpdateDate = System.DateTime.Now.ToString ();
				PrefsManager.instance.WriteData<NewsEvent> (this, PrefsManager.Kies.NewsEvent);
				MyLog.LogDebug ("wwwwwwwwwwwww");
			}
			return lastUpdateDate;
		}
		set{
			lastUpdateDate = value;
		}
	}
}
