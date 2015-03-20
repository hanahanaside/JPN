using UnityEngine;
using System.Collections;

public class StageData {

	public static readonly int NOT_CONSTRUCTION = 0;
	public static readonly int IN_CONSTRUCTION = 1;

	public enum State :int {
		Normal,
		Sleep,
		Live,
		Construction
	}

	public int Id{ get; set; }

	public int FlagConstruction{ get; set; }

	public int IdolCount{ get; set; }

	public int AreaId{ get; set; }

	public string AreaName{ get; set; }

	public string UpdatedDate{ get; set; }

	public State state{ get; set; }
}
