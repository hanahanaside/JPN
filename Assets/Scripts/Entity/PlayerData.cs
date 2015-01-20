using UnityEngine;
using System.Collections;

public class PlayerData {

	#if UNITY_EDITOR

	public int TicketCount = 0;

	public double CoinCount = 1000;

	public double TotalCoinCount = 10000;

	#else

	public int TicketCount;

	public double CoinCount;

	public double TotalCoinCount;

	#endif

	public double GenerateCoinPower;

	public string ExitDate = System.DateTime.Now.ToString();

}
