using UnityEngine;
using System.Collections;
using System;

public class SkipConstructionDialog : MonoSingleton<SkipConstructionDialog> {

	public event Action<int> PositiveButtonClickedEvent;
	public event Action NegativeButtonClickedEvent;
	public delegate void PositiveButtonClickedDelegate();
	public PositiveButtonClickedDelegate positiveButtonClicked;
	public UILabel messageLabel;
	public UILabel countLabel;
	private GameObject mDialogObject;
	private int mNeedTicketCount;

	void CompleteDismissEvent(){
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1,1,1);
	}
		
	public override void OnInitialize(){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	public void Show(int ticketCount){
		mNeedTicketCount = ticketCount;
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (mDialogObject,"ShowEvent").Play();
		messageLabel.text = "チケット" + ticketCount + "枚で完成させますか？";
		countLabel.text = "×" + ticketCount + "で";
	}

	public void NegativeButtonClicked(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		NegativeButtonClickedEvent ();
	}

	public void PositiveButtonClicked(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		PositiveButtonClickedEvent (mNeedTicketCount);
	}
}
