using UnityEngine;
using System.Collections;
using System;

public class ContinueDialogManager : MonoSingleton<ContinueDialogManager> {

	public static event Action FinishPuzzleEvent;
	public static event Action BuyTapCountEvent;
	public GameObject buyTapButtonObject;
	public UIGrid grid;
	private GameObject mDialogObject;

	public override void OnInitialize (){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	void CompleteDismissEvent(){
		mDialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1,1,1);
	}

	public void OnFinishPuzzleClicked(){
		FinishPuzzleEvent ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnBuyTapCountClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(PlayerDataKeeper.instance.TicketCount < 1){
			BuyTicketDialog.instance.Show ();
			return;
		}

		BuyTapCountEvent ();
		PlayerDataKeeper.instance.DecreaseTicketCount (1);
	}

	public void Show(){
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (gameObject,"ShowEvent").Play();
	}

	public void Dismiss(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (gameObject,"DismissEvent").Play();
	}
}
