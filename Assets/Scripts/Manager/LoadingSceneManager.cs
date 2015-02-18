using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour {

	void Start () {
	//	SoundManager.instance.StopBGM ();
	//	LoadingUIRoot.instance.ChangeBackground ();
		Invoke ("Hoge",1f);
	}

	private void Hoge(){
		Application.LoadLevel (LoadLevelName.instance.loadLevelName);
	}
}
