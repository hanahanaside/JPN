using UnityEngine;
using System.Collections;
using System;

public class MapDialogManagerTutorial : MonoSingleton<MapDialogManagerTutorial> {

	public static event Action mapClosedEvent;

	public UISprite mapSprite;
	public GameObject okButton;

	public void Show(int id){
		FenceManager.instance.ShowFence ();
		iTweenEvent.GetEvent (gameObject,"ShowEvent").Play();
		mapSprite.gameObject.SetActive (true);
		mapSprite.spriteName = "map_" + id;
		okButton.SetActive (true);
	}

	public void OKClicked(){
		mapSprite.gameObject.SetActive (false);
		okButton.SetActive (false);
		mapClosedEvent ();
	}
}
