﻿using UnityEngine;
using System.Collections;

public class BuyTicketDialog : MonoSingleton<BuyTicketDialog> {

	private GameObject mDialogObject;

	void CompleteDismissEvent(){
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public override void OnInitialize(){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	public void Show(){
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (mDialogObject,"ShowEvent").Play();
	}

	public void CloseButtonClicked(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void BuyCoinButtonClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
		BuyCoinDialog.instance.Show ();
	}

	public void BuyItem1Clicked(){
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Ticket_1);
	}

	public void BuyItem2Clicked(){
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Ticket_2);
	}

	public void BuyItem3Clicked(){
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Ticket_3);
	}

	public void BuyItem4Clicked(){
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Ticket_4);
	}
}
