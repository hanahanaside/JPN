using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;

public static class JsonParser {

	public static PlayerData DeserializePlayerData (string json) {
		PlayerData playerData = new PlayerData ();
		//データがなければ何もしない
		if (string.IsNullOrEmpty (json)) {
			return playerData;
		}
		//double の値が整数値だとCast Exceptionになる
		try{
			IDictionary playerDataDictionary = (IDictionary)Json.Deserialize (json);
			playerData.TicketCount = (int)((long)playerDataDictionary [PlayerData.Kies.TicketCount.ToString ()]);
			playerData.ExitDate = (string)playerDataDictionary [PlayerData.Kies.ExitDate.ToString ()];
			playerData.CoinCount = (double)playerDataDictionary [PlayerData.Kies.CoinCount.ToString ()];
		}catch(Exception e){
			MyLog.LogDebug (e.Message);
		}
		return playerData;
	}

	public static string SerializePlayerData (PlayerData playerData) {
		IDictionary playerDataDictionary = new Dictionary<string,object> ();
		playerDataDictionary [PlayerData.Kies.TicketCount.ToString ()] = playerData.TicketCount;
		playerDataDictionary [PlayerData.Kies.CoinCount.ToString ()] = playerData.CoinCount;
		playerDataDictionary [PlayerData.Kies.ExitDate.ToString ()] = playerData.ExitDate;
		playerDataDictionary [PlayerData.Kies.TotalCoinCount.ToString ()] = playerData.TotalCoinCount;
		string playerDataString = Json.Serialize (playerDataDictionary);
		return playerDataString;
	}
}
