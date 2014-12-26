using UnityEngine;
using System.Collections;
using System;

public class ContinueDialogManager : MonoSingleton<ContinueDialogManager> {

	public static event Action FinishPuzzleEvent;
	public static event Action BuyTapCountEvent;
	private GameObject mDialogObject;

	public override void OnInitialize (){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1,1,1);
	}

	public void OnFinishPuzzleClicked(){
		FinishPuzzleEvent ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnBuyTapCountClicked(){
		BuyTapCountEvent ();
		PlayerDataKeeper.instance.DecreaseTicketCount (1);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void Show(){
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (gameObject,"ShowEvent").Play();
	}

	public void Dismiss(){
		iTweenEvent.GetEvent (gameObject,"DismissEvent").Play();
	}
}
