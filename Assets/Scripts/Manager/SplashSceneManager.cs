using UnityEngine;
using System.Collections;

public class SplashSceneManager : MonoBehaviour {

	void OnEnable(){
		DatabaseHelper.CreatedDatabaseEvent += CreatedDatabaseEvent;
	}

	void OnDisable(){
		DatabaseHelper.CreatedDatabaseEvent -= CreatedDatabaseEvent;
	}

	void Start () {
		#if UNITY_EDITOR
		DatabaseHelper.instance.DeleteDB ();
		#endif
		DatabaseHelper.instance.CreateDB ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Hanauta);
	}

	void CreatedDatabaseEvent(){
		Invoke ("Move",2.0f);
	}

	private void Move(){
		Application.LoadLevel ("Main");
	}
}
