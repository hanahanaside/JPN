using UnityEngine;
using System.Collections;

public class PlayerDataManager : MonoSingleton<PlayerDataManager> {

	public UILabel generateCoinSpeedLabel;
	public UILabel coinCountLabel;
	public UILabel ticketCountLabel;
	private PlayerData mPlayerData;

	public double CoinCount {
		get {
			return mPlayerData.CoinCount;
		}
	}

	public double GenerateCoinSpeed {
		get {
			return mPlayerData.GenerateCoinSpeed;
		}
	}

	public void Init () {
		mPlayerData = new PlayerData ();
		mPlayerData.TicketCount = 1;
		mPlayerData.CoinCount = 10.0;
		mPlayerData.GenerateCoinSpeed = 60.0;
		UpdateGenerateSpeed (0);
		UpdateCoinCount (0);
		UpdateTicketCount (0);
	}

	public void SaveData () {

	}

	public void UpdateGenerateSpeed (double addSpeed) {
		mPlayerData.GenerateCoinSpeed += addSpeed;
		generateCoinSpeedLabel.text = GenerateCoinSpeed + "/分";
	}

	public void UpdateCoinCount (double addCoinCount) {
		if (addCoinCount > 0) {
			mPlayerData.TotalCoinCount += addCoinCount;
		}
		mPlayerData.CoinCount += addCoinCount;
		coinCountLabel.text = "" + (int)CoinCount;
	}

	public void UpdateTicketCount (int addCount) {
		mPlayerData.TicketCount += addCount;
		ticketCountLabel.text = "" + mPlayerData.TicketCount;
	}
}
