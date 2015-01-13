using UnityEngine;
using System.Collections;

public class BuyCoinDialog : MonoSingleton<BuyCoinDialog> {

	private GameObject mDialogObject;
	private GameObject mFenceObject;

	void OnEnable(){
		IAPManager.LoadFinishedEvent += LoadFinishedEvent;
	}

	void OnDisable(){
		IAPManager.LoadFinishedEvent -= LoadFinishedEvent;
	}

	void LoadFinishedEvent(){
		mFenceObject.SetActive (false);
	}

	void CompleteDismissEvent(){
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public override void OnInitialize(){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
		mFenceObject = transform.FindChild ("Fence").gameObject;
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

	public void BuyTicketButtonClicked(){
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		BuyTicketDialog.instance.Show ();
	}

	public void BuyItem1Clicked(){
		mFenceObject.SetActive (true);
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_1);
	}

	public void BuyItem2Clicked(){
		mFenceObject.SetActive (true);
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_2);
	}

	public void BuyItem3Clicked(){
		mFenceObject.SetActive (true);
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_3);
	}

	public void BuyItem4Clicked(){
		mFenceObject.SetActive (true);
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_4);
	}
}
