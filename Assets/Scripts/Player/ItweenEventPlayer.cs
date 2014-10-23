using UnityEngine;
using System.Collections;

public class ItweenEventPlayer {

	public static void PlayMoveInDialogEvent(GameObject dialogObject){
		Hashtable hashTable = new Hashtable ();
		hashTable.Add ("y",-500);
		hashTable.Add ("easetype",iTween.EaseType.easeOutBack);
		hashTable.Add ("islocal",true);
		hashTable.Add ("time",0.3);
		iTween.MoveFrom (dialogObject,hashTable);
	}

	public static void PlayMoveOutDialogEvent(GameObject dialogObject,GameObject completeTargetObject){
		Hashtable hashTable = new Hashtable ();
		hashTable.Add ("y",-500);
		hashTable.Add ("easetype",iTween.EaseType.easeInBack);
		hashTable.Add ("islocal",true);
		hashTable.Add ("time",0.3);
		hashTable.Add ("oncomplete","MoveOutEventCompleted");
		hashTable.Add ("oncompletetarget",completeTargetObject);
		iTween.MoveTo (dialogObject,hashTable);
	}
}
