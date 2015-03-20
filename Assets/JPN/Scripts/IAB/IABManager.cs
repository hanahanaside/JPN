using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IABManager : MonoSingleton<IABManager> {

	#if UNITY_ANDROID
	public static event Action LoadFinishedEvent;

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

	private const string ENCODE_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAks0WKTAHMQbotqzYHK9xlR2aB7HIH+oMW+n1KKsEyaH+5WnTjU4kLFQvPi/etNAJ5qt9iYZo1WBrx8QIqSE/sOOKDNwne2goXt5WL+OiiC2RI85tpOPzdgLBrGudhz1dGJSjJZrRs02PsJrz/+2xgf00MRwSuWc2y75KzT0nPJBs6DH1K5hFPogmnbq4hehXhyNBvwW2txY/z6ixo0GLgR75iCzWOLHcg9lgnPzzm3qEkL1Vc+iHeWYBmz2sQucRvhAxpLO0l1mMRzPXjOJ7hq2p0e4EaelNUvYkHSQh0w2FIzsurHzlD2hdQmVixGjrA9oI/xmjqW4WtNQF2r5UdwIDAQAB";
	private string[] mSKUArray = { "idol_coin_1", "idol_coin_2", "idol_coin_3", "idol_coin_4", "idol_ticket_1", "idol_ticket_2", "idol_ticket_3", "idol_ticket_4" };

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
		LoadFinishedEvent ();
	}

	void consumePurchaseSucceededEvent (GooglePurchase purchase) {
		Debug.Log ("consumePurchaseSucceededEvent: " + purchase);
		string productIdentifier = purchase.productId;
		if (productIdentifier == mSKUArray [(int)ProductId.Coin_1]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (30000);
		}
		if (productIdentifier == mSKUArray [(int)ProductId.Coin_2]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (200000);
		}
		if (productIdentifier == mSKUArray [(int)ProductId.Coin_3]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (500000);
		}
		if (productIdentifier == mSKUArray [(int)ProductId.Coin_4]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (1600000);
		}
		if (productIdentifier == mSKUArray [(int)ProductId.Ticket_1]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (10);
		}
		if (productIdentifier == mSKUArray [(int)ProductId.Ticket_2]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (75);
		}
		if (productIdentifier == mSKUArray [(int)ProductId.Ticket_3]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (200);
		}
		if (productIdentifier == mSKUArray [(int)ProductId.Ticket_4]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (500);
		}
		PlayerDataKeeper.instance.SaveData ();
		LoadFinishedEvent ();
	}

	void consumePurchaseFailedEvent (string error) {
		Debug.Log ("consumePurchaseFailedEvent: " + error);
		LoadFinishedEvent ();
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

	public void PurchaseSku (ProductId productId) {
		int index = (int)productId;
		GoogleIAB.purchaseProduct (mSKUArray[index]);
	}
	#endif
}
