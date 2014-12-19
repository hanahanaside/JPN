﻿using UnityEngine;
using System.Collections;

public class MoveStagePanelManager : MonoSingleton<MoveStagePanelManager> {

	public GameObject dialogObject;
	public UIGrid grid;
	public UIScrollView scrollView;

	void OnEnable () {
		MoveStageCell.OnMoveStageCellClickedEvent += OnMoveAreaClickedEvent;
	}

	void OnDisable () {
		MoveStageCell.OnMoveStageCellClickedEvent -= OnMoveAreaClickedEvent;
	}

	//セルクリック時に呼ばれる
	void OnMoveAreaClickedEvent (int index) {
		HideMoveStagePanel ();
		StageGridManager.instance.MoveToStage (index + 2);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	//ダイアログを閉じた時に呼ばれる
	void MoveOutEventCompleted () {
		dialogObject.transform.localPosition = new Vector3 (0, 0, 0);
		dialogObject.SetActive (false);
	}

	//グリッドを作成する
	public void CreateMoveStageGrid () {
		int stageCount = StageGridManager.instance.StageManagerList.Count;
		for (int i = 1; i <= stageCount; i++) {
			GameObject moveStageCellPrefab = Resources.Load ("MoveStageCell/MoveStageCell_" + i) as GameObject;
			GameObject moveStageCellObject = Instantiate (moveStageCellPrefab) as GameObject;
			grid.AddChild (moveStageCellObject.transform);
			moveStageCellObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}

	public void ShowMoveStagePanel () {
		if (dialogObject.activeSelf) {
			return;
		}
		FenceManager.instance.ShowFence ();
		dialogObject.SetActive (true);
		scrollView.ResetPosition ();
		ItweenEventPlayer.PlayMoveInDialogEvent (dialogObject);
	}

	public void HideMoveStagePanel () {
		if (!dialogObject.activeSelf) {
			return;
		}
		FenceManager.instance.HideFence ();
		ItweenEventPlayer.PlayMoveOutDialogEvent (dialogObject, gameObject);
	}

	public void OnFenceClicked () {
		if (dialogObject.activeSelf) {
			HideMoveStagePanel ();
		}
	}
}
