using UnityEngine;
using System.Collections;
using System;

public class FinishPuzzleDialogManager : MonoSingleton<FinishPuzzleDialogManager> {

	public static event Action BackToStageEvent;
	public static event Action<int> RetryEvent;

	public UILabel costLabel;

	private int mCost;

	public void OnBackToStageClicked(){
		BackToStageEvent();
	}

	public void OnRetryClicked(){
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		RetryEvent (0);
	}

	public void Show(){
		mCost = AreaCostCaluculator.instance.CalcCost (ScoutStageManager.SelectedAreaId -1);
		costLabel.text = "" + mCost;
	}
}
