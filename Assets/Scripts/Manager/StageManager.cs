using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {

	enum State {
		Normal,
		Sleep,
		Live}
	;

	public Transform[] fanPositionArray;
	public GameObject sleepObject;
	public UILabel untilSleepLabel;
	public float untilSleepTime;
	public double generateCoinPower;
	public int stageId;

	private const float UNTIL_GENERATE_TIME = 0.6f;
	private float mUntilSleepTime;
	private float mUntilGenerateTime = UNTIL_GENERATE_TIME;
	private double mTotalGenerateCoinPower;
	private State mState = State.Normal;

	void Start () {
		mUntilSleepTime = untilSleepTime;
		//	StageData stageData = StageDataListKeeper.instance.GetStageData (stageId-1);
		GameObject fanPrefab = Resources.Load ("Model/StageFan_1") as GameObject;
		GameObject fanObject = Instantiate (fanPrefab) as GameObject;
		fanObject.transform.parent = gameObject.transform.parent;
		fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		fanObject.transform.localPosition = fanPositionArray [0].localPosition;
	}

	void Update () {
		switch (mState) {
		case State.Normal:
			//スリープ時間を更新
			mUntilSleepTime -= Time.deltaTime;
			untilSleepLabel.text = "あと" + TimeConverter.Convert (mUntilSleepTime) + "でサボる";
			if (mUntilSleepTime < 0) {
				sleepObject.SetActive (true);
				mState = State.Sleep;
				return;
			}
			//コイン生成時間を更新
			mUntilGenerateTime -= Time.deltaTime;
			if (mUntilGenerateTime < 0) {
				PlayerDataKeeper.instance.AddCoinCount (mTotalGenerateCoinPower / 100.0);
				mUntilGenerateTime = UNTIL_GENERATE_TIME;
			}
			break;
		case State.Live:
			//コイン生成時間を更新
			mUntilGenerateTime -= Time.deltaTime;
			if (mUntilGenerateTime < 0) {
				PlayerDataKeeper.instance.AddCoinCount ((mTotalGenerateCoinPower * 2.0) / 100.0);
				mUntilGenerateTime = UNTIL_GENERATE_TIME;
			}
			break;
		case State.Sleep:
			break;
		}
	}

	public void OnWakeupButtonClicked () {
		mUntilSleepTime = untilSleepTime;
		sleepObject.SetActive (false);
		mState = State.Normal;
	}
}
