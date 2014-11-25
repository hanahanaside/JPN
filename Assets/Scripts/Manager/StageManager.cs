﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour {

	enum State {
		Normal,
		Sleep,
		Live,
		Construction
	}

	public Transform[] fanPositionArray;
	public Transform[] idlePositionArray;
	public GameObject sleepObject;
	public UILabel untilSleepLabel;
	public UILabel generateCoinPowerLabel;
	public UILabel idleCountLabel;
	public UILabel areaNameLabel;
	public UISprite idleSprite;
	public UITexture backGroundTexture;
	public AreaParams areaParams;

	private const float UNTIL_GENERATE_TIME = 0.6f;
	private float mUntilSleepTime;
	private float mUntilGenerateTime = UNTIL_GENERATE_TIME;
	private double mTotalGenerateCoinPower;
	private State mState = State.Normal;
	private StageData mStageData;
	private List<Character> mCharacterList;

	void Start () {
		mStageData = StageDataListKeeper.instance.GetStageData (areaParams.stageId - 1);
		//工事中かをチェック
		if (mStageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
			InitConstruction ();
		}else {
			InitNormal ();
		}
	}

	void Update () {
		switch (mState) {
		case State.Normal:
			//スリープ時間を更新
			mUntilSleepTime -= Time.deltaTime;
			untilSleepLabel.text = "あと" + TimeConverter.Convert (mUntilSleepTime) + "でサボる";
			if (mUntilSleepTime < 0) {
				Sleep ();
				return;
			}
			//コイン生成時間を更新
			mUntilGenerateTime -= Time.deltaTime;
			if (mUntilGenerateTime < 0) {
				PlayerDataKeeper.instance.IncreaseCoinCount (mTotalGenerateCoinPower / 100.0);
				mUntilGenerateTime = UNTIL_GENERATE_TIME;
			}
			break;
		case State.Live:
			//コイン生成時間を更新(2倍)
			mUntilGenerateTime -= Time.deltaTime;
			if (mUntilGenerateTime < 0) {
				PlayerDataKeeper.instance.IncreaseCoinCount ((mTotalGenerateCoinPower * 2.0) / 100.0);
				mUntilGenerateTime = UNTIL_GENERATE_TIME;
			}
			break;
		case State.Sleep:
			break;
		case State.Construction:
			//建設中の時間を更新
			mUntilSleepTime -= Time.deltaTime;
			untilSleepLabel.text = "あと" + TimeConverter.Convert (mUntilSleepTime) + "で完成";
			if (mUntilSleepTime > 0) {
				return;
			}
			//建設完了
			mStageData.FlagConstruction = StageData.NOT_CONSTRUCTION;
			backGroundTexture.mainTexture = Resources.Load ("Texture/St_"+mStageData.Id) as Texture;
			foreach (Character character in mCharacterList) {
				Destroy (character.gameObject);
			}
			mState = State.Normal;
			InitNormal ();
			break;
		}
	}

	public void OnWakeupButtonClicked () {
		mUntilSleepTime = areaParams.GetUntilSleepTime (mStageData.IdleCount);
		sleepObject.SetActive (false);
		mState = State.Normal;
		foreach (Character character in mCharacterList) {
			character.Wakeup ();
		}
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
	}

	private void Sleep () {
		sleepObject.SetActive (true);
		mState = State.Sleep;
		foreach (Character character in mCharacterList) {
			character.Sleep ();
		}
		PlayerDataKeeper.instance.DecreaseGenerateCoinPower (mTotalGenerateCoinPower);
	}

	public void StartLive () {
		mState = State.Live;
		if (sleepObject.activeSelf) {
			sleepObject.SetActive (false);
			PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		foreach (Character character in mCharacterList) {
			character.StartLive ();
		}
	}

	public void FinishLive () {
		mUntilSleepTime = areaParams.GetUntilSleepTime (mStageData.IdleCount);
		mState = State.Normal;
		foreach (Character character in mCharacterList) {
			character.FinishLive ();
		}
	}

	//工事中の初期化処理
	private void InitConstruction () {
		mCharacterList = new List<Character> ();
		mState = State.Construction;
		backGroundTexture.mainTexture = Resources.Load ("Texture/Construction") as Texture;
		mUntilSleepTime = TimeConverter.ConvertHoursToSeconds (areaParams.constructionTimeHours);
		//労働者を生成
		for (int i = 1; i <= 4; i++) {
			GameObject workerPrefab = Resources.Load ("Model/Worker_" + i) as GameObject;
			GameObject workerObject = Instantiate (workerPrefab) as GameObject;
			workerObject.transform.parent = gameObject.transform.parent;
			workerObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			int rand = Random.Range (0, idlePositionArray.Length);
			workerObject.transform.localPosition = idlePositionArray [rand].localPosition;
			mCharacterList.Add (workerObject.GetComponent<Character> ());
		}
	}

	//通常時の初期化処理
	private void InitNormal(){
		mCharacterList = new List<Character> ();

		//ファンを生成
		for (int i = 0; i < fanPositionArray.Length; i++) {
			GameObject fanPrefab = Resources.Load ("Model/Fan_" + (i + 1)) as GameObject;
			GameObject fanObject = Instantiate (fanPrefab) as GameObject;
			fanObject.transform.parent = gameObject.transform.parent;
			fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			fanObject.transform.localPosition = fanPositionArray [i].localPosition;
			mCharacterList.Add (fanObject.GetComponent<Character> ());
		}

		//アイドルを生成
		for (int i = 0; i < mStageData.IdleCount; i++) {
			GameObject idlePrefab = Resources.Load ("Model/Idle_" + areaParams.stageId) as GameObject;
			GameObject idleObject = Instantiate (idlePrefab) as GameObject;
			idleObject.transform.parent = gameObject.transform.parent;
			idleObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			int rand = Random.Range (0, idlePositionArray.Length);
			idleObject.transform.localPosition = idlePositionArray [rand].localPosition;
			mCharacterList.Add (idleObject.GetComponent<Character> ());
		}

		//エリア名をセット
		areaNameLabel.text = mStageData.AreaName;
		//サボるまでの時間をセット
		mUntilSleepTime = areaParams.GetUntilSleepTime (mStageData.IdleCount);
		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = areaParams.GetGeneratePower (mStageData.IdleCount) * mStageData.IdleCount;
		generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		//アイドルの数をセット
		idleCountLabel.text = "×" + mStageData.IdleCount;
		//アイドルの画像をセット
		idleSprite.spriteName = "idle_normal_" + areaParams.stageId;
		UISpriteData spriteData = idleSprite.GetAtlasSprite ();
		idleSprite.SetDimensions (spriteData.width, spriteData.height);
	}
}
