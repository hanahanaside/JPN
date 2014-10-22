using UnityEngine;
using System.Collections;

public class MovePanelManager : MonoBehaviour {

	public  GameObject listView;

	void MoveOutEventFinished(){
	//	transform.parent.gameObject.SetActive (false);
	}

	public void MoveIn () {
		transform.parent.gameObject.SetActive (true);
		iTweenEvent.GetEvent (listView, "MoveInEvent").Play ();
	}

	public void MoveOut(){
		iTweenEvent.GetEvent (listView, "MoveOutEvent").Play ();
	}
}
