using UnityEngine;
using System.Collections;
using System;
using System.Text;

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
		StageData	stage = dao.SelectById (mIdleId);
		mDialogObject.SetActive (true);
		mIdleSprite.spriteName = "idle_normal_" + id;
		UISpriteData spriteData = mIdleSprite.GetAtlasSprite ();
		mIdleSprite.SetDimensions (spriteData.width, spriteData.height);

		StringBuilder sb = new System.Text.StringBuilder ();
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

	private string GetUntilLevelUpMessage (StageData stage) {
		int untilLevelUpCount = 0;
		string untilLevelUpMessage = "";
		if(stage.IdleCount >= 25){
			untilLevelUpMessage = CreateUntilLevelUpMessage (stage, 0, "レベルMAX");
		}else if (stage.IdleCount > 21) {
			untilLevelUpCount = 25 - stage.IdleCount;
			untilLevelUpMessage = CreateUntilLevelUpMessage (stage, untilLevelUpCount, "レベルMAX");
		} else if (stage.IdleCount > 16) {
			untilLevelUpCount = 21 - stage.IdleCount;
			untilLevelUpMessage = CreateUntilLevelUpMessage (stage, untilLevelUpCount, "レベル5");
		} else if (stage.IdleCount > 11) {
			untilLevelUpCount = 16 - stage.IdleCount;
			untilLevelUpMessage = CreateUntilLevelUpMessage (stage, untilLevelUpCount, "レベル4");
		} else if (stage.IdleCount > 6) {
			untilLevelUpCount = 11 - stage.IdleCount;
			untilLevelUpMessage = CreateUntilLevelUpMessage (stage, untilLevelUpCount, "レベル3");
		} else {
			untilLevelUpCount = 6 - stage.IdleCount;
			untilLevelUpMessage = CreateUntilLevelUpMessage (stage, untilLevelUpCount, "レベル2");
		}
		return untilLevelUpMessage;
	}

	//CreateUntilLevelUpMessage(対象のステージデータ, 次のレベルまでのカウント, 次のレベルの名前)
	private string CreateUntilLevelUpMessage (StageData stage, int untilLevelUpCount, string level) {
		StringBuilder sb = new StringBuilder ();
		if (untilLevelUpCount <= 0) {
			GenerateCoinPowerDao generateCoinPowerDao = DaoFactory.CreateGenerateCoinPowerDao ();
			UntilSleepTimeDao untilSleepTimeDao = DaoFactory.CreateUntilSleepTimeDao ();
			double generateCoinPower = generateCoinPowerDao.SelectById (stage.Id, stage.IdleCount);
			int untilSleepTimeMin = untilSleepTimeDao.SelectById (stage.Id, stage.IdleCount);
			sb.Append ("(" + level + "にアップ！)\n");
			sb.Append ("収入ペースが" + generateCoinPower + "にUP!!\n");
			sb.Append ("サボるまでの時間が" + untilSleepTimeMin + "分にUP!!");
			return sb.ToString ();
		}
		sb.Append (level + "まであと" + untilLevelUpCount + "人");
		return sb.ToString ();
	}
}
