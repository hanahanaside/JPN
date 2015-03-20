using UnityEngine;
using System.Collections;

public class MapDialogManager : MonoSingleton<MapDialogManager> {

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
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		mapSprite.gameObject.SetActive (false);
		FenceManager.instance.HideFence ();
	}

}
