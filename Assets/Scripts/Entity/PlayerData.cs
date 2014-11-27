using UnityEngine;
using System.Collections;

public class PlayerData {

	public enum Kies{
		TicketCount,
		CoinCount,
		TotalCoinCount,
		ExitDate
	}

	public int TicketCount{ get; set; }

	public double CoinCount{ get; set; }

	public double TotalCoinCount{ get; set; }

	public string ExitDate{ get; set; }
}
