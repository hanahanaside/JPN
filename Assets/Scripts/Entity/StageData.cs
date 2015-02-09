using UnityEngine;
using System.Collections;

public class StageData {

	public enum StateType {
		Normal,
		Sleep,
		Live,
		Construction
	}

	public int Id{ get; set; }

	public int IdleCount{ get; set; }

	public string AreaName{ get; set; }

	public string UpdatedDate{ get; set; }

	public StateType State{ get; set; }
}
