using UnityEngine;
using System.Collections;

public class BuyCoinDialog : MonoSingleton<BuyCoinDialog> {

	private GameObject mDialogObject;
	private GameObject mFrontFenceObject;
	private GameObject mBackFenceObject;

	void OnEnable () {
		#if UNITY_IPHONE
		IAPManager.LoadFinishedEvent += LoadFinishedEvent;
		#endif
		#if UNITY_ANDROID
		IABManager.LoadFinishedEvent += LoadFinishedEvent;
		#endif
	}

	void OnDisable () {
		#if UNITY_IPHONE
		IAPManager.LoadFinishedEvent -= LoadFinishedEvent;
		#endif
		#if UNITY_ANDROID
		IABManager.LoadFinishedEvent -= LoadFinishedEvent;
		#endif
	}

	void LoadFinishedEvent () {
		mFrontFenceObject.SetActive (false);
	}

	void CompleteDismissEvent () {
		mDialogObject.SetActive (false);
		mBackFenceObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	public override void OnInitialize () {
		mDialogObject = transform.FindChild ("Dialog").gameObject;
		mFrontFenceObject = transform.FindChild ("FrontFence").gameObject;
		mBackFenceObject = transform.FindChild ("BackFence").gameObject;
	}

	public void Show () {
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (true);
		mBackFenceObject.SetActive (true);
		iTweenEvent.GetEvent (mDialogObject, "ShowEvent").Play ();
	}

	public void CloseButtonClicked () {
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void BuyTicketButtonClicked () {
		iTweenEvent.GetEvent (mDialogObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		BuyTicketDialog.instance.Show ();
	}

	public void BuyItem1Clicked () {
		mFrontFenceObject.SetActive (true);
		#if UNITY_IPHONE
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_1);
		#endif
		#if UNITY_ANDROID
		IABManager.instance.PurchaseSku(IABManager.ProductId.Coin_1);
		#endif
	}

	public void BuyItem2Clicked () {
		mFrontFenceObject.SetActive (true);
		#if UNITY_IPHONE
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_2);
		#endif
		#if UNITY_ANDROID
		IABManager.instance.PurchaseSku(IABManager.ProductId.Coin_2);
		#endif
	}

	public void BuyItem3Clicked () {
		mFrontFenceObject.SetActive (true);
		#if UNITY_IPHONE
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_3);
		#endif
		#if UNITY_ANDROID
		IABManager.instance.PurchaseSku(IABManager.ProductId.Coin_3);
		#endif
	}

	public void BuyItem4Clicked () {
		mFrontFenceObject.SetActive (true);
		#if UNITY_IPHONE
		IAPManager.instance.PurchaseItem (IAPManager.ProductId.Coin_4);
		#endif
		#if UNITY_ANDROID
		IABManager.instance.PurchaseSku(IABManager.ProductId.Coin_4);
		#endif
	}
}
