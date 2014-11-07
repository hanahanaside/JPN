using UnityEngine;
using System.Collections;

public class ScoutStageManager : MonoSingleton<ScoutStageManager> {

	public Transform[] areaPositionArray;
	public GameObject dartsObject;
	public GameObject planeObject;
	public GameObject fadeOutSpriteObject;
	public GameObject goScoutButtonObject;

	void OnEnable(){
		AreaPanelManager.instance.OnAreaClickedEvent += OnAreaClickedEvent;
	}

	void OnDisable(){
		AreaPanelManager.instance.OnAreaClickedEvent -= OnAreaClickedEvent;
	}

	void OnAreaClickedEvent(int areaIndexNumber){
		dartsObject.transform.localPosition = areaPositionArray [areaIndexNumber].localPosition;
		dartsObject.SetActive (true);
	}

	void OnPlaneEventCompleted(){
		fadeOutSpriteObject.SetActive (true);
	}

	public static bool FlagScouting{ get; set;}

	public void StartLive(){

	}

	public void OnFadeOutFinished(){
		Application.LoadLevel ("Puzzle");
	}

	public void OnAreaButtonClicked(){
		dartsObject.SetActive (false);
		AreaPanelManager.instance.ShowAreaPanel ();
	}

	public void OnGoScoutButtonClicked(){
		if(!dartsObject.activeSelf){
			return;
		}
		goScoutButtonObject.SetActive (false);
		FlagScouting = true;
		iTweenEvent.GetEvent (planeObject,"moveOut").Play();
	}

	public void PlayMoveInPlaneAnimation(){
		iTweenEvent.GetEvent (planeObject,"moveIn").Play();
	}
}
