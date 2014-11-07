using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour {

	enum State {
		Normal,
		Sleep,
		Live}
	;

	public Transform[] fanPositionArray;
	public Transform[] idlePositionArray;
	public GameObject sleepObject;
	public UILabel untilSleepLabel;
	public UILabel generateCoinPowerLabel;
	public UILabel idleCountLabel;
	public UILabel areaNameLabel;
	public UISprite idleSprite;
	public AreaParams areaParams;

	private const float UNTIL_GENERATE_TIME = 0.6f;
	private float mUntilSleepTime;
	private float mUntilGenerateTime = UNTIL_GENERATE_TIME;
	private double mTotalGenerateCoinPower;
	private State mState = State.Normal;
	private List<Idle> mIdleList;

	void Start () {
		mIdleList = new List<Idle> ();
		StageData stageData = StageDataListKeeper.instance.GetStageData (areaParams.stageId - 1);

		//ファンを生成
		GameObject fanPrefab = Resources.Load ("Model/Fan_1") as GameObject;
		GameObject fanObject = Instantiate (fanPrefab) as GameObject;
		fanObject.transform.parent = gameObject.transform.parent;
		fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		fanObject.transform.localPosition = fanPositionArray [0].localPosition;

		//アイドルを生成
		for (int i = 0; i < stageData.IdleCount; i++) {
			GameObject idlePrefab = Resources.Load ("Model/Idle_" + areaParams.stageId) as GameObject;
			GameObject idleObject = Instantiate (idlePrefab) as GameObject;
			idleObject.transform.parent = gameObject.transform.parent;
			idleObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			int rand = Random.Range (0, idlePositionArray.Length);
			idleObject.transform.localPosition = idlePositionArray [rand].localPosition;
			mIdleList.Add (idleObject.GetComponent<Idle> ());
		}

		//エリア名をセット
		areaNameLabel.text = stageData.AreaName;
		//サボるまでの時間をセット
		mUntilSleepTime = areaParams.GetUntilSleepTime (mIdleList.Count);
		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = areaParams.GetGeneratePower (mIdleList.Count) * stageData.IdleCount;
		generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		//アイドルの数をセット
		idleCountLabel.text = "×" + mIdleList.Count;
		//アイドルの画像をセット
		idleSprite.spriteName = "idle_normal_" + areaParams.stageId;
		UISpriteData spriteData = idleSprite.GetAtlasSprite ();
		idleSprite.SetDimensions (spriteData.width, spriteData.height);
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
		}
	}

	public void OnWakeupButtonClicked () {
		mUntilSleepTime = areaParams.GetUntilSleepTime (mIdleList.Count);
		sleepObject.SetActive (false);
		mState = State.Normal;
		foreach (Idle idle in mIdleList) {
			idle.Wakeup ();
		}
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
	}

	private void Sleep () {
		sleepObject.SetActive (true);
		mState = State.Sleep;
		foreach (Idle idle in mIdleList) {
			idle.Sleep ();
		}
		PlayerDataKeeper.instance.DecreaseGenerateCoinPower (mTotalGenerateCoinPower);
	}

	public void StartLive () {
		mState = State.Live;
		if (sleepObject.activeSelf) {
			sleepObject.SetActive (false);
			PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		foreach (Idle idle in mIdleList) {
			idle.StartLive ();
		}
	}

	public void FinishLive () {
		mUntilSleepTime = areaParams.GetUntilSleepTime (mIdleList.Count);
		mState = State.Normal;
		foreach (Idle idle in mIdleList) {
			idle.FinishLive ();
		}
	}
}
