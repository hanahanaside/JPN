using UnityEngine;
using System.Collections;

public class SplashSceneManager : MonoBehaviour {

	void Start () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Hanauta);
		Invoke ("Move",2.0f);
	}

	private void Move(){
		Application.LoadLevel ("Main");
	}
}
