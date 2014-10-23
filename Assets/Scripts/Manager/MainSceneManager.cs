using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainSceneManager : MonoSingleton<MainSceneManager> {

	void Start () {
		StageDataListKeeper.instance.LoadData ();
		PlayerDataKeeper.instance.Init ();
		StageGridManager.instance.CreateStageGrid ();
		if(ScoutManager.FlagScouting){
			StageGridManager.instance.MoveToStage (0);
			ScoutManager.instance.PlayMoveInPlaneAnimation ();
		}else {
			StageGridManager.instance.MoveToStage (1);
			EventManager.instance.Init ();
		}
	}

	void OnApplicationPause(bool pauseStatus){
		if(pauseStatus){

		}else {
			PlayerDataKeeper.instance.SaveData ();
			StageDataListKeeper.instance.SaveData ();
		}
	}

	public void OnScoutButtonClicked(){
		MoveStagePanelManager.instance.HideMoveStagePanel ();
		StageGridManager.instance.MoveToStage (0);
	}

	public void OnManagementButtonClicked(){
		ManagementPanelManager.instance.ShowManagementPanel ();
	}

	public void OnMoveStageButtonClicked(){
		MoveStagePanelManager.instance.ShowMoveStagePanel ();
	}

	public void OnMenuButtonClicked(){
		MenuPanelManager.instance.ShowMenuPanel ();
	}

}
