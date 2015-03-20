using UnityEngine;
using System.Collections;
using System;

public class MoveStageCell : MonoBehaviour {

	public static event Action<int> OnMoveStageCellClickedEvent;

	public void Init(StageData stage){
		UIButton button = GetComponent<UIButton> ();
		button.normalSprite = "puzzle_idle_" + stage.Id;
		UILabel label = transform.FindChild ("Label").GetComponent<UILabel>();
		label.text = stage.AreaName; 
	}

	public void OnClick(){
		UIGrid grid = transform.parent.GetComponent<UIGrid> ();
		int index = grid.GetIndex (transform);
		OnMoveStageCellClickedEvent (index);
	}
}
