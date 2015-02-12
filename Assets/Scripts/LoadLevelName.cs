using UnityEngine;
using System.Collections;

public class LoadLevelName : MonoSingleton<LoadLevelName> {

	public string loadLevelName{ get; set; }

	public override void OnInitialize () {
		DontDestroyOnLoad (gameObject);
	}
}
