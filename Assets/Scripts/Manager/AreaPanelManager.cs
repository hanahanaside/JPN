using UnityEngine;
using System.Collections;
using System;

public class AreaPanelManager : MonoSingleton<AreaPanelManager> {

	public event Action<int> OnAreaClickedEvent;
	public GameObject dialogObject;

	void MoveOutEventCompleted(){
		dialogObject.transform.localPosition = new Vector3 (0,0,0);
		dialogObject.SetActive (false);
	}

	public void ShowAreaPanel(){
		dialogObject.SetActive (true);
		ItweenEventPlayer.PlayMoveInDialogEvent (dialogObject);
	}

	public void OnCloseButtonClicked(){
		ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject,gameObject);
		OnAreaClickedEvent (3);
	}

}
