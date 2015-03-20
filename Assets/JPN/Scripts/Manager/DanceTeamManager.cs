using UnityEngine;
using System.Collections;

public class DanceTeamManager : MonoBehaviour {

	private int mIdolCount;

	public void StartDancing (int stageId, int idolCount) {
		mIdolCount = idolCount;
		foreach (Transform childTransform in transform) {
			ChangeIdolSprite (stageId, childTransform);
		}
	}

	private void ChangeIdolSprite (int stageId, Transform childTransform) {
		foreach (Transform grandChildTransform in childTransform) {
			if (mIdolCount <= 0) {
				grandChildTransform.gameObject.SetActive (false);
				continue;
			}
			UISprite sprite = grandChildTransform.GetComponent<UISprite> ();
			sprite.spriteName = "idle_normal_" + stageId;
			UISpriteData spriteData = sprite.GetAtlasSprite ();
			sprite.SetDimensions (spriteData.width, spriteData.height);
			mIdolCount--;
		}
	}
}
