using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StageManagerTutorial : MonoBehaviour {

	public static event Action<GameObject> FinishConstructionEvent;
	public static event Action<GameObject> WakeupEvent;

	public enum State {
		Normal,
		Sleep,
		Live,
		Construction
	}

	public UILabel untilSleepLabel;
	public UILabel generateCoinPowerLabel;
	public UILabel idleCountLabel;
	public UILabel areaNameLabel;
	public UISprite idleSprite;
	public GameObject danceTeamPrefab;

	private const float UNTIL_GENERATE_TIME = 0.6f;
	private float mTimeSeconds;
	private float mUntilGenerateTime = UNTIL_GENERATE_TIME;
	private double mTotalGenerateCoinPower;
	private State mState = State.Normal;
	private StageData mStageData;
	private List<Character> mCharacterList = new List<Character> ();
	private GameObject mSkipConstructionButtonObject;
	private GameObject sleepObject;
	private GameObject mDanceTeamObject;
	private GameObject mContainerObject;
	private UITexture backGroundTexture;

	public void Init (StageData stage) {
		mStageData = stage;
		mSkipConstructionButtonObject = transform.Find ("Container/SkipConstructionButton").gameObject;
		mContainerObject = transform.FindChild ("Container").gameObject;
		sleepObject = transform.Find ("Container/Sleep").gameObject;
		backGroundTexture = GetComponentInChildren<UITexture> ();
		//工事中かをチェック
		if (mStageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
			InitConstruction ();
		} else {
			InitNormal ();
		}
	}

	void Update () {
		switch (mState) {
		case State.Normal:
			//スリープ時間を更新
			mTimeSeconds -= Time.deltaTime;
			untilSleepLabel.text = "あと" + TimeConverter.Convert (mTimeSeconds) + "でサボる";
			if (mTimeSeconds < 0) {
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
				PlayerDataKeeper.instance.IncreaseCoinCount ((mTotalGenerateCoinPower * 2.0f) / 100.0);
				mUntilGenerateTime = UNTIL_GENERATE_TIME;
			}
			//建設中の場合の処理
			if (mStageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
				mTimeSeconds -= Time.deltaTime * 2.0f;
				if (mTimeSeconds >= 0) {
					untilSleepLabel.text = "あと" + TimeConverter.Convert (mTimeSeconds) + "で完成";
				}
			}
			break;
		case State.Sleep:
			break;
		case State.Construction:
			//建設中の時間を更新
			mTimeSeconds -= Time.deltaTime;
			untilSleepLabel.text = "あと" + TimeConverter.Convert (mTimeSeconds) + "で完成";
			if (mTimeSeconds > 0) {
				return;
			}
			//建設完了
			backGroundTexture.mainTexture = Resources.Load ("Texture/St_" + mStageData.Id) as Texture;
			foreach (Character character in mCharacterList) {
				Destroy (character.gameObject);
			}
			mState = State.Normal;
			mStageData.FlagConstruction = StageData.NOT_CONSTRUCTION;
			mStageData.UpdatedDate = DateTime.Now.ToString ();
			DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
			mCharacterList = new List<Character> ();
			InitNormal ();
			FinishConstructionEvent (gameObject);
			MyLog.LogDebug ("finish construction stage " + mStageData.Id);
			break;
		}
	}
		
	//再開時の処理
	public void Resume () {
		//工事中かをチェック
		if (mStageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
			SetConstructionTime ();
		} else {
			//サボるまでの時間をセット(テストで10分の1)
			SetUntilSleepTime ();
		}
	}
		
	//喝ボタン押下時の処理
	public void OnWakeupButtonClicked () {
		mTimeSeconds = GetUntilSleepTime () * 60;
		mStageData.UpdatedDate = DateTime.Now.ToString ();
		DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
		sleepObject.SetActive (false);
		mState = State.Normal;
		gameObject.tag = "default";
		foreach (Character character in mCharacterList) {
			character.Wakeup ();
		}
		//画像を変更
		idleSprite.spriteName = "idle_normal_" + mStageData.Id;
		UISpriteData spriteData = idleSprite.GetAtlasSprite ();
		idleSprite.SetDimensions (spriteData.width, spriteData.height);
		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = GetGenerateCoinPower ();
		generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Katsu);
		WakeupEvent (gameObject);
		MyLog.LogDebug ("wake up stage " + mStageData.Id);
	}

	//サボりを開始
	private void Sleep () {
		gameObject.tag = "sleep";
		sleepObject.SetActive (true);
		mState = State.Sleep;
		//コイン生成パワーをセット
		generateCoinPowerLabel.text = "0/分";
		//サボるまでの時間をセット
		untilSleepLabel.text = "サボり中";
		//画像を変更
		idleSprite.spriteName = "idle_sleep_" + mStageData.Id;
		UISpriteData spriteData = idleSprite.GetAtlasSprite ();
		idleSprite.SetDimensions (spriteData.width, spriteData.height);

		foreach (Character character in mCharacterList) {
			character.Sleep ();
		}
		PlayerDataKeeper.instance.DecreaseGenerateCoinPower (mTotalGenerateCoinPower);
		MyLog.LogDebug ("sleep stage " + mStageData.Id);
	}

	//ライブを開始
	public void StartLive () {
		mState = State.Live;
		mSkipConstructionButtonObject.SetActive (false);
		untilSleepLabel.text = "LIVE！！！！！！！！！！！";
		gameObject.tag = "default";
		if (sleepObject.activeSelf) {
			sleepObject.SetActive (false);
			//画像を変更
			idleSprite.spriteName = "idle_normal_" + mStageData.Id;
			UISpriteData spriteData = idleSprite.GetAtlasSprite ();
			idleSprite.SetDimensions (spriteData.width, spriteData.height);
			PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		//ライブ時は2倍
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		foreach (Character character in mCharacterList) {
			character.StartLive ();
		}
		if (mStageData.FlagConstruction != StageData.IN_CONSTRUCTION) {
			mDanceTeamObject = Instantiate (danceTeamPrefab)as GameObject;
			mDanceTeamObject.transform.parent = mContainerObject.transform;
			mDanceTeamObject.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
			mDanceTeamObject.transform.localPosition = new Vector3 (20, 10, 0);
			DanceTeamManager danceTeamManager = mDanceTeamObject.GetComponent<DanceTeamManager> ();
			danceTeamManager.StartDancing (mStageData.Id, mStageData.IdolCount);
			generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower * 2) + "/分";
		} 
	}

	//ライブを終了
	public void FinishLive () { 
		//ライブ中で終了した場合は倍になっている数字を元に戻す
		if (mState == State.Live) {
			PlayerDataKeeper.instance.DecreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		if (mStageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
			mState = State.Construction;
			mSkipConstructionButtonObject.SetActive (true);
		} else {
			mTimeSeconds = GetUntilSleepTime () * 60;
			generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
			mState = State.Normal;
			//ダンスチームのインスタンスを削除
			if (mDanceTeamObject != null) {
				Destroy (mDanceTeamObject);
				mDanceTeamObject = null;
			}
		}
		foreach (Character character in mCharacterList) {
			character.FinishLive ();
		}
	}
		
	//今すぐ完成させるボタン押下
	public void SkipConstructionClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		//チュートリアルなので1枚
		int ticketCount = 1;
		SkipConstructionDialogTutorial.instance.Show (ticketCount);
		SkipConstructionDialogTutorial.instance.positiveButtonClicked = () => {
			mTimeSeconds = 0;
			PlayerDataKeeper.instance.DecreaseTicketCount (ticketCount);
		};
	}
		
	//工事中の初期化処理
	private void InitConstruction () {
		gameObject.tag = "construction";

		mCharacterList = new List<Character> ();
		mState = State.Construction;
		//背景を設置
		backGroundTexture.mainTexture = Resources.Load ("Texture/Construction") as Texture;

		//建設時間を設置(テストで10分の1)
		SetConstructionTime ();

		//アイドルの画像をセット
		idleSprite.spriteName = "idle_normal_" + mStageData.Id;
		UISpriteData spriteData = idleSprite.GetAtlasSprite ();
		idleSprite.SetDimensions (spriteData.width, spriteData.height);

		//アイドルの数をセット
		SetIdolCount ();

		//エリア名をセット
		areaNameLabel.text = "建設中";

		//コイン生成パワーをセット
		generateCoinPowerLabel.text = "0/分";

		//今すぐ完成させるボタンを表示
		mSkipConstructionButtonObject.SetActive (true);

		//労働者を生成
		for (int i = 1; i <= 4; i++) {
			GameObject workerPrefab = Resources.Load ("Model/Worker/Worker_" + i) as GameObject;
			GameObject workerObject = Instantiate (workerPrefab) as GameObject;
			workerObject.transform.parent = transform;
			workerObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			float x = UnityEngine.Random.Range (-175.0f, 175.0f);
			float y = UnityEngine.Random.Range (0, 300.0f);
			workerObject.transform.localPosition = new Vector3 (x, y, 0);
			mCharacterList.Add (workerObject.GetComponent<Character> ());
			workerObject.GetComponent<Worker> ().Init ();
		}
	}

	//通常時の初期化処理
	private void InitNormal () {

		GameObject idlePrefab = Resources.Load ("Model/Idle/Idle_" + mStageData.Id) as GameObject;
		//アイドルを生成
		for (int i = 0; i < mStageData.IdolCount; i++) {
			GenerateIdle (idlePrefab);
		}

		//ファンを生成
		for (int i = 0; i < mStageData.IdolCount * 3; i++) {
			int rand = UnityEngine.Random.Range (1, 14);
			GameObject fanPrefab = Resources.Load ("Model/Fan/Fan_" + rand) as GameObject;
			GameObject fanObject = Instantiate (fanPrefab) as GameObject;
			float x = UnityEngine.Random.Range (-250.0f, 250.0f);
			float y = UnityEngine.Random.Range (-230.0f, -180.0f);
			fanObject.transform.parent = mContainerObject.transform;
			fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			fanObject.transform.localPosition = new Vector3 (x, y, 0);
			mCharacterList.Add (fanObject.GetComponent<Character> ());
			fanObject.GetComponent<Fan> ().Init ();
		}

		//エリア名をセット
		areaNameLabel.text = mStageData.AreaName;

		//サボるまでの時間をセット(テストで10分の1)
		SetUntilSleepTime ();

		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = GetGenerateCoinPower ();
		generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);

		//アイドルの数をセット
		SetIdolCount ();

		//今すぐ完成させるボタンを非表示
		mSkipConstructionButtonObject.SetActive (false);

		//背景をセット
		backGroundTexture.mainTexture = Resources.Load ("Texture/St_" + mStageData.Id) as Texture;

		//アイドルの画像をセット
		idleSprite.spriteName = "idle_normal_" + mStageData.Id;
		UISpriteData spriteData = idleSprite.GetAtlasSprite ();
		idleSprite.SetDimensions (spriteData.width, spriteData.height);
	}

	//アイドルを生成
	private GameObject GenerateIdle (GameObject idlePrefab) {
		GameObject idleObject = Instantiate (idlePrefab) as GameObject;
		idleObject.transform.parent = mContainerObject.transform;
		idleObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		float x = UnityEngine.Random.Range (-175.0f, 175.0f);
		float y = UnityEngine.Random.Range (0, 300.0f);
		idleObject.transform.localPosition = new Vector3 (x, y, 0);
		idleObject.GetComponent<Idle> ().Init ();
		mCharacterList.Add (idleObject.GetComponent<Character> ());
		return idleObject;
	}

	//建設中の時間をセット
	private void SetConstructionTime () {
		ConstructionTimeDao dao = DaoFactory.CreateConstructionTimeDao ();
		float constructionTimeSeconds = dao.SelectById (mStageData.Id) * 60;  
		float timeSpanSeconds = TimeSpanCalculator.CalcFromNow (mStageData.UpdatedDate);
		mTimeSeconds = constructionTimeSeconds - timeSpanSeconds;
	}

	//サボるまでの時間をセット
	private void SetUntilSleepTime () {
		float untilSleepTimeSeconds = GetUntilSleepTime () * 60;
		float timeSpanSeconds = TimeSpanCalculator.CalcFromNow (mStageData.UpdatedDate);
		mTimeSeconds = untilSleepTimeSeconds - timeSpanSeconds;
	}

	//サボるまでの時間をDBから取得
	private int GetUntilSleepTime () {
		UntilSleepTimeDao dao = DaoFactory.CreateUntilSleepTimeDao ();
		return dao.SelectById (mStageData.Id, mStageData.IdolCount);
	}

	//コイン生成パワーをDBから取得
	private double GetGenerateCoinPower () {
		GenerateCoinPowerDao dao = DaoFactory.CreateGenerateCoinPowerDao ();
		return dao.SelectById (mStageData.Id, mStageData.IdolCount);
	}

	//アイドルの人数をセット
	private void SetIdolCount () {
		if (mStageData.IdolCount >= 25) {
			idleCountLabel.text = "MAX";
		} else {
			idleCountLabel.text = "×" + mStageData.IdolCount;
		}
	}

}
