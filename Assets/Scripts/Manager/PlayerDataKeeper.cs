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
		generateCoinSpeedLabel.text = GameMath.RoundOne(mGenerateCoinPower) + "/分";
		coinCountLabel.text = "" + GameMath.RoundZero(mPlayerData.CoinCount);
		ticketCountLabel.text = ""+mPlayerData.TicketCount;
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
	}

	public double GenerateCoinPower{
		get{
			return mGenerateCoinPower;
		}
	}
		
	public void Init () {
		string playerDataJson = PrefsManager.instance.PlayerDataJson;
		MyLog.LogDebug ("init player data " + playerDataJson);
		mPlayerData = JsonParser.DeserializePlayerData (playerDataJson);
	}

	public void SaveData () {
		mPlayerData.ExitDate = DateTime.Now.ToString ();
		string playerDataJson = JsonParser.SerializePlayerData (mPlayerData);
		PrefsManager.instance.PlayerDataJson = playerDataJson;
		MyLog.LogDebug ("save player data " + playerDataJson);
	}

	public void IncreaseGenerateCoinPower(double coinPower){
		mGenerateCoinPower += coinPower;
	}

	public void IncreaseCoinCount(double coinCount){
		mPlayerData.CoinCount += coinCount;
	}

	public void IncreaseTicketCount(int increaseCount){
		mPlayerData.TicketCount += increaseCount;
	}
		
	public void DecreaseGenerateCoinPower(double coinPower){
		mGenerateCoinPower -= coinPower;
	}
}
