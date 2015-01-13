using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPManager : MonoSingleton<IAPManager> {

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

	private string[] mProductIdentifiers = { "idol_coin_1", "idol_coin_2", "idol_coin_3", "idol_coin_4", "idol_ticket_1", "idol_ticket_2", "idol_ticket_3", "idol_ticket_4" };
	private List<StoreKitProduct> mProductsList;

	void OnEnable () {
		StoreKitManager.transactionUpdatedEvent += transactionUpdatedEvent;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent += purchaseFailedEvent;
		StoreKitManager.productListReceivedEvent += productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent += productListRequestFailedEvent;
	}

	void OnDisable () {
		StoreKitManager.transactionUpdatedEvent -= transactionUpdatedEvent;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent -= purchaseFailedEvent;
		StoreKitManager.productListReceivedEvent -= productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent -= productListRequestFailedEvent;
	}

	void transactionUpdatedEvent (StoreKitTransaction transaction) {
		Debug.Log ("transactionUpdatedEvent: " + transaction);
	}


	void productListReceivedEvent (List<StoreKitProduct> productList) {
		Debug.Log ("productListReceivedEvent. total products received: " + productList.Count);
		mProductsList = productList;
	}


	void productListRequestFailedEvent (string error) {
		Debug.Log ("productListRequestFailedEvent: " + error);
	}


	void purchaseFailedEvent (string error) {
		Debug.Log ("purchaseFailedEvent: " + error);
	}


	void purchaseCancelledEvent (string error) {
		Debug.Log ("purchaseCancelledEvent: " + error);
	}

	void purchaseSuccessfulEvent (StoreKitTransaction transaction) {
		Debug.Log ("purchaseSuccessfulEvent: " + transaction);
		string productIdentifier = transaction.productIdentifier;
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Coin_1]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (10000);
		}
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Coin_2]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (75000);
		}
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Coin_3]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (200000);
		}
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Coin_4]) {
			PlayerDataKeeper.instance.IncreaseCoinCount (500000);
		}
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Ticket_1]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (10);
		}
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Ticket_2]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (75);
		}
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Ticket_3]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (200);
		}
		if (productIdentifier == mProductIdentifiers [(int)ProductId.Ticket_4]) {
			PlayerDataKeeper.instance.IncreaseTicketCount (500);
		}
	}

	public override void OnInitialize () {
		DontDestroyOnLoad (gameObject);
		bool canMakePayments = StoreKitBinding.canMakePayments ();
		Debug.Log ("StoreKit canMakePayments: " + canMakePayments);
		if (canMakePayments) {
			StoreKitBinding.requestProductData (mProductIdentifiers);
			Debug.Log ("request product data");
		}
	}

	public void PurchaseItem (ProductId productId) { 
		if (mProductsList == null) {
			return;
		}
		int index = (int)productId;
		StoreKitProduct product = mProductsList [index];
		StoreKitBinding.purchaseProduct (product.productIdentifier, 1);
	}
}
