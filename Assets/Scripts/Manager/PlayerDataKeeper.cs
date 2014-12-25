using UnityEngine;
using System.Collections;
using MiniJSON;
using System;

public class PlayerDataKeeper : MonoSingleton<PlayerDataKeeper> {

	public UILabel generateCoinSpeedLabel;
	public UILabel coinCountLabel;
	public UILabel ticketCountLabel;
	private PlayerData mPlayerData;
	private double mGenerateCoinPower;

	void Update(){
		if(Application.loadedLevelName == "Main"){
			generateCoinSpeedLabel.text = GameMath.RoundOne(mGenerateCoinPower) + "/分";
		}else {
			generateCoinSpeedLabel.text = "";
		}

		coinCountLabel.text = "" + GameMath.RoundZero(mPlayerData.CoinCount);
		ticketCountLabel.text = ""+mPlayerData.TicketCount;
		mPlayerData.GenerateCoinPower = mGenerateCoinPower;
	}

	public double CoinCount {
		get {
			return mPlayerData.CoinCount;
		}
	}

	public double TotalCoinCount{
		get{
			return mPlayerData.TotalCoinCount;
		}
	}

	public string ExitDate{
		get{
			return mPlayerData.ExitDate;
		}
		set{
			mPlayerData.ExitDate = value;
		}
	}

	public double GenerateCoinPower{
		get{
			return mGenerateCoinPower;
		}
	}

	public double SavedGenerateCoinPower{
		get{
			return mPlayerData.GenerateCoinPower;
		}
	}
		
	public void Init () {
		mPlayerData = Resources.Load ("Data/PlayerData") as PlayerData;
	}
		
	public void IncreaseGenerateCoinPower(double coinPower){
		mGenerateCoinPower += coinPower;
	}

	public void IncreaseCoinCount(double coinCount){
		mPlayerData.CoinCount += coinCount;
		mPlayerData.TotalCoinCount += coinCount;
	}

	public void IncreaseTicketCount(int increaseCount){
		mPlayerData.TicketCount += increaseCount;
	}
		
	public void DecreaseGenerateCoinPower(double coinPower){
		mGenerateCoinPower -= coinPower;
	}

	public void DecreaseCoinCount(double coinCount){
		mPlayerData.CoinCount -= coinCount;
	}

	public void DecreaseTicketCount(int decreaseCount){
		mPlayerData.TicketCount -= decreaseCount;
	}
}
