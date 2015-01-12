using UnityEngine;
using System.Collections;
using System;

public class GetIdleDialogManager : MonoSingleton<GetIdleDialogManager> {

	public static event Action ClosedEvent;

	public GameObject debutButton;
	public GameObject tradeButton;

	private UILabel mMessageLabel;
	private UISprite mIdleSprite;
	private int mIdleId;
	private int mTradeCost;
	private GameObject mDialogObject;

	void CompleteDismissEvent () {
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1, 1, 1);
		ClosedEvent ();
	}

	public override void OnInitialize () {
		mDialogObject = transform.FindChild ("Dialog").gameObject;
		mIdleSprite = mDialogObject.transform.FindChild ("IdleSprite").GetComponent<UISprite> ();
		mMessageLabel = mDialogObject.transform.FindChild ("MessageLabel").GetComponent<UILabel> ();
	}

	public void DebutButtonClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
	}

	public void TradeButtonClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
		PlayerDataKeeper.instance.IncreaseCoinCount (mTradeCost);
	}

	public void Show (int id) {
		mIdleId = id;
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage	stage = dao.SelectById (mIdleId);
		mDialogObject.SetActive (true);
		mIdleSprite.spriteName = "idle_normal_" + id;
		mIdleSprite.width = mIdleSprite.GetAtlasSprite ().width * 2;
		mIdleSprite.height = mIdleSprite.GetAtlasSprite ().height * 2;

		System.Text.StringBuilder sb = new System.Text.StringBuilder ();
		if (stage.IdleCount >= 25) {
			debutButton.SetActive (false);
			tradeButton.SetActive (true);
			int cost = AreaCostCaluculator.instance.CalcCost (ScoutStageManager.SelectedAreaId - 1);
			mTradeCost = (int)(cost * 0.2);
			UILabel buttonLabel = tradeButton.GetComponentInChildren<UILabel> ();
			buttonLabel.text = "×" + mTradeCost + "で移籍させます";
			sb.Append ("人数オーバー");
		} else {
			debutButton.SetActive (true);
			tradeButton.SetActive (false);
			stage.IdleCount++;
			if (string.IsNullOrEmpty (stage.UpdatedDate)) {
				stage.UpdatedDate = DateTime.Now.ToString ();
			}
			dao.UpdateRecord (stage);
			sb.Append (stage.AreaName + "の子をスカウトした！");
			sb.Append ("\n");
			sb.Append (stage.IdleCount + " / 25\n");
			sb.Append (GetUntilLevelUpMessage (stage));

		}
			
		mMessageLabel.text = sb.ToString ();
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}

	private string GetUntilLevelUpMessage (Stage stage) {
		int untilLevelUpCount = 0;
		string untilLevelUpMessage = "";
		if (stage.IdleCount > 20) {
			untilLevelUpCount = 25 - stage.IdleCount;
			untilLevelUpMessage = "レベルMAXまであと" + untilLevelUpCount + "人";
		} else if (stage.IdleCount > 15) {
			untilLevelUpCount = 20 - stage.IdleCount;
			untilLevelUpMessage = "レベル5まであと" + untilLevelUpCount + "人";
		} else if (stage.IdleCount > 10) {
			untilLevelUpCount = 15 - stage.IdleCount;
			untilLevelUpMessage = "レベル4まであと" + untilLevelUpCount + "人";
		} else if (stage.IdleCount > 5) {
			untilLevelUpCount = 10 - stage.IdleCount;
			untilLevelUpMessage = "レベル3まであと" + untilLevelUpCount + "人";
		} else {
			untilLevelUpCount = 5 - stage.IdleCount;
			untilLevelUpMessage = "レベル2まであと" + untilLevelUpCount + "人";
		}

		if (untilLevelUpCount == 0) {
			untilLevelUpMessage = "レベルアップ！！";
		}
		return untilLevelUpMessage;
	}
}
