using UnityEngine;
using System.Collections;
using System;

public class FinishPuzzleDialogManager : MonoBehaviour {

	public static event Action BackToStageEvent;
	public static event Action<int> RetryEvent;

	public void OnBackToStageClicked(){
		BackToStageEvent();
	}

	public void OnRetryClicked(){
		RetryEvent (0);
	}

	public void Show(){

	}
}
