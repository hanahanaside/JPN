using UnityEngine;
using System.Collections;

public class FenceManager : MonoSingleton<FenceManager> {

	public GameObject fenceSpriteObject;

	public void ShowFence(){
		fenceSpriteObject.SetActive (true);
	}

	public void HideFence(){
		fenceSpriteObject.SetActive (false);
	}
}
