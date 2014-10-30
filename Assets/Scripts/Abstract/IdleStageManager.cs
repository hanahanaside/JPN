using UnityEngine;
using System.Collections;

public class IdleStageManager<T> : StageManager<T> where T : IdleStageManager<T> {

	public GameObject idlePrefab;
	public UITexture backGroundTexture;

	public Texture2D BackGroundTexture{
		set{
			backGroundTexture.mainTexture = value;
		}
	}
}
