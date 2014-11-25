using UnityEngine;
using System.Collections;

public class StageData {

	public static readonly int NOT_CONSTRUCTION = 0;
	public static readonly int IN_CONSTRUCTION = 1;

	public int Id{ get; set; }

	public int FlagConstruction{ get; set; }

	public int IdleCount{ get; set; }

	public string AreaName{ get; set; }

	public string CreatedDate{ get; set; }
	//作られた日付
}
