using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IABManager : MonoSingleton<IABManager> {

	#if UNITY_ANDROID
	public enum ProductId {
		Coin_1,
		Coin_2,
		Coin_3,
		Coin_4,
		Ticket_1,
		Ticket_2,
		Ticket_3,
		Ticket_4}
	;

	private const string ENCODE_PUBLIC_KEY = "";
	private string[] mSKUArray = { };

	void OnEnable () {
		// Listen to all events for illustration purposes
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	}

	void OnDisable () {
		// Remove all event handlers
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}

	public override void OnInitialize () {
		DontDestroyOnLoad (gameObject);
		GoogleIAB.init (ENCODE_PUBLIC_KEY);
	}

	void purchaseFailedEvent (string error, int response) {
		Debug.Log ("purchaseFailedEvent: " + error);
	}

	void consumePurchaseSucceededEvent (GooglePurchase purchase) {
		Debug.Log ("consumePurchaseSucceededEvent: " + purchase);
	}

	void consumePurchaseFailedEvent (string error) {
		Debug.Log ("consumePurchaseFailedEvent: " + error);
	}

	void billingSupportedEvent () {
		Debug.Log ("billingSupportedEvent");
		GoogleIAB.queryInventory (mSKUArray);
	}

	void billingNotSupportedEvent (string error) {
		Debug.Log ("billingNotSupportedEvent: " + error);
	}

	void queryInventorySucceededEvent (List<GooglePurchase> purchases, List<GoogleSkuInfo> skus) {
		Debug.Log (string.Format ("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
		Prime31.Utils.logObject (purchases);
		Prime31.Utils.logObject (skus);
	}

	void queryInventoryFailedEvent (string error) {
		Debug.Log ("queryInventoryFailedEvent: " + error);
	}

	void purchaseCompleteAwaitingVerificationEvent (string purchaseData, string signature) {
		Debug.Log ("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	void purchaseSucceededEvent (GooglePurchase purchase) {
		Debug.Log ("purchaseSucceededEvent: " + purchase);
		Debug.Log ("id = " + purchase.productId);
		GoogleIAB.consumeProduct (purchase.productId);

	}

	public void PurchaseSku (int skuIndex) {
		string sku = mSKUArray [skuIndex];
		Debug.Log ("sku = " + sku);
		GoogleIAB.purchaseProduct (sku);
	}
	#endif
}
