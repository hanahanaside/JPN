using UnityEngine;
using System.Collections;

public class MapDialogManager : MonoSingleton<MapDialogManager> {

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
		FenceManager.instance.HideFence ();
	}

}
