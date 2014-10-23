using UnityEngine;
using System.Collections;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	public void OnBackButtonClicked () {
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}
		

}
