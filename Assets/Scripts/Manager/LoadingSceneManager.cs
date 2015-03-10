using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour {

	void Start () {
		SoundManager.instance.StopBGM ();
		Application.LoadLevel (LoadLevelName.instance.loadLevelName);
	}
		
}
