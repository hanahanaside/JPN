using UnityEngine;
using System.Collections;

public class ScoutManager : MonoSingleton<ScoutManager> {

	public GameObject dartsObject;
	public GameObject planeObject;
	public GameObject fadeOutPanel;

	void OnPlaneEventCompleted(){
		fadeOutPanel.SetActive (true);
	}

	public static bool FlagScouting{ get; set;}

	public void OnFadeOutFinished(){
		Application.LoadLevel ("Puzzle");
	}

	public void OnAreaButtonClicked(){
		dartsObject.SetActive (true);
	}

	public void OnGoScoutButtonClicked(){
		if(!dartsObject.activeSelf){
			return;
		}
		FlagScouting = true;
		iTweenEvent.GetEvent (planeObject,"moveOut").Play();
	}

	public void PlayMoveInPlaneAnimation(){
		iTweenEvent.GetEvent (planeObject,"moveIn").Play();
	}
}
