using UnityEngine;
using System.Collections;

public class PlayerDataKeeper : MonoSingleton<PlayerDataKeeper> {

	public UILabel generateCoinSpeedLabel;
	public UILabel coinCountLabel;
	public UILabel ticketCountLabel;
	private PlayerData mPlayerData;

	void Update(){
		generateCoinSpeedLabel.text = GameMath.RoundOne(mPlayerData.GenerateCoinPower) + "/分";
		coinCountLabel.text = "" + GameMath.RoundZero(mPlayerData.CoinCount);
		ticketCountLabel.text = ""+mPlayerData.TicketCount;
	}

	public double CoinCount {
		get {
			return mPlayerData.CoinCount;
		}
	}
		
	public void Init () {
		mPlayerData = new PlayerData ();
		mPlayerData.TicketCount = 1;
	}

	public void SaveData () {

	}

	public void IncreaseGenerateCoinPower(double coinPower){
		mPlayerData.GenerateCoinPower += coinPower;
	}

	public void IncreaseCoinCount(double coinCount){
		mPlayerData.CoinCount += coinCount;
	}
		
	public void DecreaseGenerateCoinPower(double coinPower){
		mPlayerData.GenerateCoinPower -= coinPower;
	}
}
