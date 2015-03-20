using UnityEngine;
using System.Collections;

public class LoadingUIRoot : MonoSingleton<LoadingUIRoot> {

	public Texture2D[] backgroundTextureArray;

	public void ChangeBackground () {
		int rand = UnityEngine.Random.Range (0, backgroundTextureArray.Length);
		transform.GetComponentInChildren<UITexture> ().mainTexture = backgroundTextureArray [rand];
	}
}
