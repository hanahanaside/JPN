using UnityEngine;
using System.Collections;
using System;

public class FinishPuzzleDialogManager : MonoSingleton<FinishPuzzleDialogManager> {

	public static event Action BackToStageEvent;
	public static event Action<int> RetryEvent;

	public GameObject retryButtonObject;
	public UILabel costLabel;
	public UIGrid grid;

	private int mCost;

	private GameObject mDialogObject;

	public override void OnInitialize (){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1,1,1);
	}

	public void OnBackToStageClicked(){
		BackToStageEvent();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnRetryClicked(){
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		RetryEvent (0);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void Show(){
		mDialogObject.SetActive (true);
		mCost = AreaCostCaluculator.instance.CalcCost (ScoutStageManager.SelectedAreaId -1);
		if(PlayerDataKeeper.instance.CoinCount < mCost){
			retryButtonObject.SetActive (false);
			grid.Reposition ();
		}
		iTweenEvent.GetEvent (gameObject,"ShowEvent").Play();
		costLabel.text = "" + mCost;
	}

	public void Dismiss(){
		iTweenEvent.GetEvent (gameObject,"DismissEvent").Play();
	}
}
