using UnityEngine;
using System.Collections;
using System;

public class FinishPuzzleDialogManager : MonoBehaviour {

	public static event Action FinishPuzzleEvent;

	public void OnFinishPuzzleButtonClicked(){
		FinishPuzzleEvent();
	}
}
