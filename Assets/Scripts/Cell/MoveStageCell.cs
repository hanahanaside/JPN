using UnityEngine;
using System.Collections;
using System;

public class MoveStageCell : MonoBehaviour {

	public static event Action<int> OnMoveStageCellClickedEvent;

	public void OnClick(){
		UIGrid grid = characterTtransform.parent.GetComponent<UIGrid> ();
		int index = grid.GetIndex (characterTtransform);
		OnMoveStageCellClickedEvent (index);
	}
}
