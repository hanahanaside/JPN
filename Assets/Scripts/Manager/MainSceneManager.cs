using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainSceneManager : MonoSingleton<MainSceneManager> {

	void Start () {
		StageDataListKeeper.instance.Init ();
		StageGridManager.instance.CreateStageGrid ();
		StageGridManager.instance.MoveToStage (1);
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
