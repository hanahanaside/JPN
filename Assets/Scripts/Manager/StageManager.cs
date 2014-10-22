using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {

	public UITexture backGroundTexture;

	void Start () {
		int indexNumber = StageGridManager.instance.GetIndexNumber (transform.parent);
		StageData stageData = StageDataListKeeper.instance.GetStageData (indexNumber - 2);
		Texture2D texture2D = Resources.Load ("Textures/St_" + stageData.Id) as Texture2D;
		backGroundTexture.mainTexture = texture2D;
	}

}
