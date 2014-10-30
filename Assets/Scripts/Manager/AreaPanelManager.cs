using UnityEngine;
using System.Collections;
using System;

public class AreaPanelManager : MonoSingleton<AreaPanelManager> {

	public event Action<int> OnAreaClickedEvent;
	public UIScrollView areaScrollView;
	public GameObject dialogObject;

	void MoveOutEventCompleted(){
		dialogObject.transform.localPosition = new Vector3 (0,0,0);
		dialogObject.SetActive (false);
	}

	public void ShowAreaPanel(){
		dialogObject.SetActive (true);
		areaScrollView.ResetPosition ();
		ItweenEventPlayer.PlayMoveInDialogEvent (dialogObject);
	}

	public void OnHokkaidoClicked(){
		ClosePanel (0);
	}

	public void OnTohokuClicked(){
		ClosePanel (1);
	}

	public void OnKantoClicked(){
		ClosePanel (2);
	}

	public void OnChubuClicked(){
		ClosePanel (3);
	}

	public void OnKinkiClicked(){
		ClosePanel (4);
	}

	public void OnChugokuClicked(){
		ClosePanel (5);
	}

	public void OnShikokuClicked(){
		ClosePanel (6);
	}

	public void OnKyushuClicked(){
		ClosePanel (7);
	}

	public void OnCloseButtonClicked(){
		ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject,gameObject);
	}

	private void ClosePanel(int areaIndex){
		ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject,gameObject);
		OnAreaClickedEvent (areaIndex);
	}
}
