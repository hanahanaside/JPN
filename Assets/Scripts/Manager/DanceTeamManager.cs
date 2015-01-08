using UnityEngine;
using System.Collections;

public class DanceTeamManager : MonoBehaviour {

	public void StartDancing (int stageId) {
		foreach (Transform childTransform in transform) {
			ChangeIdolSprite (stageId, childTransform);
		}
	}

	private void ChangeIdolSprite (int stageId, Transform childTransform) {
		foreach (Transform grandChildTransform in childTransform) {
			UISprite sprite = grandChildTransform.GetComponent<UISprite> ();
			sprite.spriteName = "idle_normal_" + stageId;
			UISpriteData spriteData = sprite.GetAtlasSprite ();
			sprite.SetDimensions (spriteData.width, spriteData.height);
		}
	}
}
