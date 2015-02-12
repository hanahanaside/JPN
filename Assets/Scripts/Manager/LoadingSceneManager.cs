using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour {

	void Start () {
		SoundManager.instance.StopBGM ();
		LoadingUIRoot.instance.ChangeBackground ();
		Application.LoadLevel (LoadLevelName.instance.loadLevelName);
	}
}
