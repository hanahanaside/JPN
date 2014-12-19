using UnityEngine;
using System.Collections;
using System;

public class ContinueDialogManager : MonoBehaviour {

	public static event Action FinishPuzzleEvent;
	public static event Action BuyTapCountEvent;

	public void OnFinishPuzzleClicked(){
		FinishPuzzleEvent ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnBuyTapCountClicked(){
		BuyTapCountEvent ();
		PlayerDataKeeper.instance.DecreaseTicketCount (1);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}
}
