using UnityEngine;
using System.Collections;

public class IdolStageContainer : MonoBehaviour {

	private StageManager mStageManager;
	private IdolStageStatusManager mIdolStageStatusManager;
	private UITexture mBackgroundTexture;
	private GameObject mSkipConstructionButtonObject;
	private GameObject mSleepObject;

	public void FindObjects () {
		mStageManager = GetComponentInParent<StageManager> ();
		mIdolStageStatusManager = GetComponentInChildren<IdolStageStatusManager> ();
		mBackgroundTexture = GetComponentInChildren<UITexture> ();
		mSkipConstructionButtonObject = transform.FindChild ("SkipConstructionButton").gameObject;
		mSleepObject = transform.FindChild ("Sleep").gameObject;
		mIdolStageStatusManager.FindObjects ();
	}

	//喝ボタン押下時の処理
	public void WakeupButtonClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Katsu);
		transform.FindChild ("Sleep").gameObject.SetActive (false);
		//アイコン広告を非表示にする
		AdManager.instance.HideIconAd ();
		mStageManager.Wakeup ();
	}

	//今すぐ完成させるボタン押下
	public void SkipConstructionButtonClicked () {
		mStageManager.SkipConstruction ();
	}

	//アイドルの画像を変更する
	public void ChangeIdolSprite (string spriteName) {
		mIdolStageStatusManager.IdolSpriteName = spriteName;
	}

	public void ChangeBackgroundTexture (string resourcePath) {
		mBackgroundTexture.mainTexture = Resources.Load (resourcePath) as Texture;
	}

	//背景の画像を変更する
	public void SetUntilSleepLabel (string labelText) {
		mIdolStageStatusManager.UntilSleepLabel = labelText;
	}

	//コイン生成パワーのラベルをセットする
	public void SetGenerateCoinPowerLabel (string labelText) {
		mIdolStageStatusManager.GenerateCoinPowerLabel = labelText;
	}

	//エリア名のラベルをセットする
	public void SetAreaNameLabel (string labelText) {
		mIdolStageStatusManager.AreaNameLabel = labelText;
	}

	//アイドルの数のラベルをセットする
	public void SetIdolCountLabel (StageData stageData) {
		if (stageData.IdolCount >= 25) {
			mIdolStageStatusManager.IdolCountLabel = "MAX";
		} else {
			mIdolStageStatusManager.IdolCountLabel = "×" + stageData.IdolCount;
		}
	}

	//今すぐ完成ボタンを表示する
	public void ShowSkipConstructionButton () {
		mSkipConstructionButtonObject.SetActive (true);
	}

	//今すぐ完成ボタンを非表示にする
	public void HideSkipConstructionButton () {
		mSkipConstructionButtonObject.SetActive (false);
	}

	//サボる画像を全て表示する
	public void ShowSleepObjects () {
		mSleepObject.SetActive (true);
	}

	//サボる画像を全て非表示にする
	public void HideSleepObjects () {
		mSleepObject.SetActive (false);
	}

	//コンテナ自身を表示する
	public void ShowContainer () {
		gameObject.SetActive (true);
	}

	//コンテナ自身を非表示にする
	public void HideContainer () {
		gameObject.SetActive (false);
	}

	//通常時のUI処理
	public void SetNormal(StageData stageData,double generateCoinPower){
		SetAreaNameLabel (stageData.AreaName);
		SetGenerateCoinPowerLabel (GameMath.RoundOne (generateCoinPower) + "/分");
		SetIdolCountLabel (stageData);
		HideSkipConstructionButton ();
		ChangeBackgroundTexture ("Texture/St_" + stageData.Id);
		ChangeIdolSprite ("idle_normal_" + stageData.Id);
	}

	//サボった時のUI処理
	public void SetSleep (StageData stageData) {
		SetGenerateCoinPowerLabel ("0/分");
		SetUntilSleepLabel ("サボり中");
		ChangeIdolSprite ("idle_sleep_" + stageData.Id);
	}

	//建設時のUI処理
	public void SetConstruction (StageData stageData) {
		ChangeBackgroundTexture ("Texture/Construction");
		ChangeIdolSprite ("idle_normal_" + stageData.Id);
		SetIdolCountLabel (stageData);
		SetAreaNameLabel ("建設中");
		SetGenerateCoinPowerLabel ("0/分");
		ShowSkipConstructionButton ();
	}

	//コンテナが表示中であればtrueを返す
	public bool IsContainerShowing () {
		return gameObject.activeSelf;
	}

	//サボる画像が全て表示中であればtrueを返す
	public bool IsSleepObjectsShowing () {
		return mSleepObject.activeSelf;
	}
}
