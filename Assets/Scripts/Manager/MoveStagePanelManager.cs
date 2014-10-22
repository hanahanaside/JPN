using UnityEngine;
using System.Collections;

public class MoveStagePanelManager : MonoSingleton<MoveStagePanelManager> {

	public GameObject moveStagePanelObject;

	void MoveOutEventFinished(){
		moveStagePanelObject.transform.localPosition = new Vector3 (0,0,0);
		moveStagePanelObject.SetActive (false);
	}

	public void ShowMoveStagePanel () {
		if(moveStagePanelObject.activeSelf){
			return;
		}
		FenceManager.instance.ShowFence ();
		moveStagePanelObject.SetActive (true);
		iTweenEvent.GetEvent (moveStagePanelObject, "MoveInEvent").Play ();
	}

	public void HideMoveStagePanel(){
		if(!moveStagePanelObject.activeSelf){
			return;
		}
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (moveStagePanelObject, "MoveOutEvent").Play ();
	}

	public void OnFenceClicked(){
		if(moveStagePanelObject.activeSelf){
			HideMoveStagePanel ();
		}
	}
}
