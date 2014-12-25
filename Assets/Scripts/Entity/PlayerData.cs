using UnityEngine;
using System.Collections;

public class PlayerData {

	public enum Kies{
		TicketCount,
		CoinCount,
		TotalCoinCount,
		ExitDate,
		GenerateCoinPower
	}

	public int TicketCount{ get; set; }

	public double CoinCount{ get; set; }

	public double TotalCoinCount{ get; set; }

	public double GenerateCoinPower{ get; set;}

	public string ExitDate{ get; set; }
}
