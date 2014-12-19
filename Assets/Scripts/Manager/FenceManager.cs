using UnityEngine;
using System.Collections;

public class FenceManager : MonoSingleton<FenceManager> {

	public GameObject fenceSpriteObject;
	public GameObject transparentFenceObject;

	public void ShowFence(){
		fenceSpriteObject.SetActive (true);
	}

	public void ShowTransparentFence(){
		transparentFenceObject.SetActive (true);
	}

	public void HideTransparentFence(){
		transparentFenceObject.SetActive (false);
	}

	public void HideFence(){
		fenceSpriteObject.SetActive (false);
	}
}
