using UnityEngine;
using System.Collections;

public class IdolStageStatus : MonoBehaviour {

	private UILabel mUntilSleepLabel;
	private UILabel mGenerateCoinPowerLabel;
	private UILabel mIdolCountLabel;
	private UILabel mAreaNameLabel;
	private UISprite mIdolSprite;

	public void FindObjects(){
		mUntilSleepLabel = transform.FindChild ("UntilSleepLabel").GetComponent<UILabel>();
		mGenerateCoinPowerLabel = transform.FindChild ("GenerateCoinPowerLabel").GetComponent<UILabel>();
		mIdolCountLabel = transform.FindChild ("IdolCountLabel").GetComponent<UILabel>();
		mAreaNameLabel = transform.FindChild ("AreaNameLabel").GetComponent<UILabel>();
		mIdolSprite = transform.FindChild ("IdolSprite").GetComponent<UISprite>();
	}

	public string UntilSleepLabel {
		set {
			mUntilSleepLabel.text = value;
		}
	}

	public string GenerateCoinPowerLabel {
		set {
			mGenerateCoinPowerLabel.text = value;
		}
	}

	public string IdolCountLabel {
		set {
			mIdolCountLabel.text = value;
		}
	}

	public string AreaNameLabel {
		set {
			mAreaNameLabel.text = value;
		}
	}

	public string IdolSpriteName {
		set {
			mIdolSprite.spriteName = value;
			UISpriteData spriteData = mIdolSprite.GetAtlasSprite ();
			mIdolSprite.SetDimensions (spriteData.width, spriteData.height);
		}
	}
}
