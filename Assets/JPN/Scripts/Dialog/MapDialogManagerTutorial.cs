using UnityEngine;
using System.Collections;
using System;

public class MapDialogManagerTutorial : MonoSingleton<MapDialogManagerTutorial> {

	public static event Action mapClosedEvent;

	public UISprite mapSprite;

	public void Show(int id){
		FenceManager.instance.ShowFence ();
		iTweenEvent.GetEvent (gameObject,"ShowEvent").Play();
		mapSprite.gameObject.SetActive (true);
		mapSprite.spriteName = "map_" + id;
	}

	public void FenceClicked(){
		if(!mapSprite.gameObject.activeSelf){
			return;
		}
		mapSprite.gameObject.SetActive (false);
		mapClosedEvent ();
	}
}
