using UnityEngine;
using System.Collections;
using System;

public class ContinueDialogManager : MonoBehaviour {

	public static event Action FinishPuzzleEvent;
	public static event Action BuyTapCountEvent;

	public void OnFinishPuzzleClicked(){
		FinishPuzzleEvent ();
	}

	public void OnBuyTapCountClicked(){
		BuyTapCountEvent ();
	}
}
