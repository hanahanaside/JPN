using UnityEngine;
using System.Collections;

public class MoveStagePanelManager : MonoSingleton<MoveStagePanelManager> {

	public GameObject dialogObject;

	void MoveOutEventCompleted(){
		dialogObject.transform.localPosition = new Vector3 (0,0,0);
		dialogObject.SetActive (false);
	}

	public void ShowMoveStagePanel () {
		if(dialogObject.activeSelf){
			return;
		}
		FenceManager.instance.ShowFence ();
		dialogObject.SetActive (true);
		ItweenEventPlayer.PlayMoveInDialogEvent (dialogObject);
	}

	public void HideMoveStagePanel(){
		if(!dialogObject.activeSelf){
			return;
		}
		FenceManager.instance.HideFence ();
		ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject,gameObject);
	}

	public void OnFenceClicked(){
		if(dialogObject.activeSelf){
			HideMoveStagePanel ();
		}
	}
}
