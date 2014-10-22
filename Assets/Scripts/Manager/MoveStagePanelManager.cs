using UnityEngine;
using System.Collections;

public class MoveStagePanelManager : MonoSingleton<MoveStagePanelManager> {

	public GameObject moveStagePanelObject;
	public  GameObject listView;

	void MoveOutEventFinished(){
		listView.transform.localPosition = new Vector3 (0,0,0);
		moveStagePanelObject.SetActive (false);
	}

	public void ShowMoveStagePanel () {
		if(moveStagePanelObject.activeSelf){
			return;
		}
		FenceManager.instance.ShowFence ();
		moveStagePanelObject.SetActive (true);
		iTweenEvent.GetEvent (listView, "MoveInEvent").Play ();
	}

	public void HideMoveStagePanel(){
		if(!moveStagePanelObject.activeSelf){
			return;
		}
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (listView, "MoveOutEvent").Play ();
	}

	public void OnFenceClicked(){
		if(moveStagePanelObject.activeSelf){
			HideMoveStagePanel ();
		}
	}
}
