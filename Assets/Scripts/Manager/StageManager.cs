using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StageManager : MonoBehaviour {

	public static event Action SleepEvent;
	public static event Action WakeupEvent;

	enum State {
		Normal,
		Sleep,
		Live,
		Construction
	}
		
	public GameObject sleepObject;
	public UILabel untilSleepLabel;
	public UILabel generateCoinPowerLabel;
	public UILabel idleCountLabel;
	public UILabel areaNameLabel;
	public UISprite idleSprite;
	public UITexture backGroundTexture;
	public AreaParams areaParams;

	private const float UNTIL_GENERATE_TIME = 0.6f;
	private float mTimeSeconds;
	private float mUntilGenerateTime = UNTIL_GENERATE_TIME;
	private double mTotalGenerateCoinPower;
	private State mState = State.Normal;
	private Stage mStageData;
	private List<Character> mCharacterList = new List<Character> ();

	void OnEnable () {
		Idle.FoundEvent += FoundIdleEvent;
	}

	void OnDisable () {
		Idle.FoundEvent -= FoundIdleEvent;
	}

	public void Init () {
		generateCoinPowerLabel.color = new Color (0.19f, 0.58f, 0.78f, 1.0f);
		mStageData = DaoFactory.CreateStageDao ().SelectById (areaParams.stageId);
		idleCountLabel.color = new Color (0.1f, 0.7f, 0.6f, 1.0f);
		//工事中かをチェック
		if (mStageData.FlagConstruction == Stage.IN_CONSTRUCTION) {
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
			//コイン生成時間を更新(10倍)
			mUntilGenerateTime -= Time.deltaTime;
			if (mUntilGenerateTime < 0) {
				PlayerDataKeeper.instance.IncreaseCoinCount ((mTotalGenerateCoinPower * 2.0f) / 100.0);
				mUntilGenerateTime = UNTIL_GENERATE_TIME;
			}
			//建設中の場合の処理
			if (mStageData.FlagConstruction == Stage.IN_CONSTRUCTION) {
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
			mStageData.FlagConstruction = Stage.NOT_CONSTRUCTION;
			mStageData.UpdatedDate = DateTime.Now.ToString ();
			DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
			mCharacterList = new List<Character> ();
			InitNormal ();
			MyLog.LogDebug ("finish construction stage " + mStageData.Id);
			break;
		}
	}

	void FoundIdleEvent (Character character) {
		mCharacterList.Remove (character);
	}

	//再開時の処理
	public void Resume () {
		//工事中かをチェック
		if (mStageData.FlagConstruction == Stage.IN_CONSTRUCTION) {
			SetConstructionTime ();
		} else {
			//サボるまでの時間をセット(テストで10分の1)
			SetUntilSleepTime ();
		}
	}

	//喝ボタン押下時の処理
	public void OnWakeupButtonClicked () {
		mTimeSeconds = areaParams.GetUntilSleepTimeMinutes (mStageData.IdleCount) * 60;
	//	mTimeSeconds = (areaParams.GetUntilSleepTimeMinutes (mStageData.IdleCount) * 60) / 10;
		mStageData.UpdatedDate = DateTime.Now.ToString ();
		DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
		sleepObject.SetActive (false);
		mState = State.Normal;
		transform.parent.gameObject.tag = "default";
		foreach (Character character in mCharacterList) {
			character.Wakeup ();
		}
		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = areaParams.GetGeneratePower (mStageData.IdleCount);
		generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		WakeupEvent ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Katsu);
		MyLog.LogDebug ("wake up stage " + mStageData.Id);
	}

	//サボりを開始
	private void Sleep () {
		transform.parent.gameObject.tag = "sleep";
		sleepObject.SetActive (true);
		mState = State.Sleep;
		//コイン生成パワーをセット
		generateCoinPowerLabel.text = "0/分";
		//サボるまでの時間をセット
		untilSleepLabel.text = "サボり中";
		foreach (Character character in mCharacterList) {
			character.Sleep ();
		}
		PlayerDataKeeper.instance.DecreaseGenerateCoinPower (mTotalGenerateCoinPower);
		SleepEvent ();
		MyLog.LogDebug ("sleep stage " + mStageData.Id);
	}

	//ライブを開始
	public void StartLive () {
		mState = State.Live;
		untilSleepLabel.text = "LIVE！！！！！！！！！！！";
		transform.parent.gameObject.tag = "default";
		if (sleepObject.activeSelf) {
			sleepObject.SetActive (false);
			WakeupEvent ();
			PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		foreach (Character character in mCharacterList) {
			character.StartLive ();
		}
		if (mStageData.FlagConstruction != Stage.IN_CONSTRUCTION) {
			GameObject danceTeamPrefab = Resources.Load<GameObject> ("DanceTeam/DanceTeam");
			GameObject danceTeamObject = Instantiate (danceTeamPrefab)as GameObject;
			danceTeamObject.transform.parent = transform.parent;
			danceTeamObject.transform.localScale = new Vector3 (0.6f,0.6f,0.6f);
			danceTeamObject.transform.localPosition = new Vector3 (20,10,0);
			DanceTeamManager danceTeamManager = danceTeamObject.GetComponent<DanceTeamManager> ();
			danceTeamManager.StartDancing (areaParams.stageId,mStageData.IdleCount);
			generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower * 2) + "/分";
		} 
	}

	//ライブを終了
	public void FinishLive () { 
		if (mStageData.FlagConstruction == Stage.IN_CONSTRUCTION) {
			mState = State.Construction;
		} else {
			mTimeSeconds = areaParams.GetUntilSleepTimeMinutes (mStageData.IdleCount) * 60;
		//	mTimeSeconds = (areaParams.GetUntilSleepTimeMinutes (mStageData.IdleCount) * 60) / 10;
			generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
			mState = State.Normal;
			GameObject danceTeamObject = transform.parent.Find ("DanceTeam(Clone)").gameObject;
			Destroy (danceTeamObject);
		}
		foreach (Character character in mCharacterList) {
			character.FinishLive ();
		}
		mStageData.UpdatedDate = DateTime.Now.ToString ();
		DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
	}

	//アイドル発見時に追加する処理
	public void AddIdle (int count) {
		GameObject idlePrefab = Resources.Load ("Model/Idle/Idle_" + mStageData.Id) as GameObject; 
		for (int i = 0; i < count; i++) {
			GenerateIdle (idlePrefab);
		}
		mStageData = DaoFactory.CreateStageDao ().SelectById (areaParams.stageId);
		idleCountLabel.text = "×" + mStageData.IdleCount;
		if (mState == State.Sleep) {
			foreach (Character character in mCharacterList) {
				character.Sleep ();
			}
		}
	}

	public void RemoveIdle (int count) {
		for (int i = 0; i < count; i++) {
			Character character = mCharacterList [0];
			mCharacterList.Remove (character);
			Destroy (character.gameObject);
		}
		mStageData = DaoFactory.CreateStageDao ().SelectById (areaParams.stageId);
		idleCountLabel.text = "×" + mStageData.IdleCount;
	}
		
	//迷子のアイドルを生成
	public void GenerateLostIdle (int idleId) {
		GameObject lostIdlePrefab = Resources.Load ("Model/Idle/Idle_" + idleId) as GameObject;
		GameObject lostIdleObject =	GenerateIdle (lostIdlePrefab);
		BoxCollider boxCollider = lostIdleObject.AddComponent<BoxCollider> ();
		boxCollider.isTrigger = true;
		boxCollider.size = new Vector3 (150, 150, 0);
		GameObject exPrefab = Resources.Load ("GUI/EX") as GameObject;
		GameObject exObject = Instantiate (exPrefab) as GameObject;
		exObject.transform.parent = lostIdleObject.transform;
		exObject.transform.localScale = new Vector3 (1,1,1);
		exObject.transform.localPosition = new Vector3 (0,80,0);
	}

	//工事中の初期化処理
	private void InitConstruction () {
		transform.parent.gameObject.tag = "construction";

		mCharacterList = new List<Character> ();
		mState = State.Construction;
		//背景を設置
		backGroundTexture.mainTexture = Resources.Load ("Texture/Construction") as Texture;

		//建設時間を設置(テストで10分の1)
		SetConstructionTime ();

		//労働者の画像をセット
		idleSprite.spriteName = "worker_1";

		//労働者の数をセット
		idleCountLabel.text = "";

		//エリア名をセット
		areaNameLabel.text = "建設中";

		//コイン生成パワーをセット
		generateCoinPowerLabel.text = "0/分";

		//労働者を生成
		for (int i = 1; i <= 4; i++) {
			GameObject workerPrefab = Resources.Load ("Model/Worker/Worker_" + i) as GameObject;
			GameObject workerObject = Instantiate (workerPrefab) as GameObject;
			workerObject.transform.parent = gameObject.transform.parent;
			workerObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			float x = UnityEngine.Random.Range (-175.0f, 175.0f);
			float y = UnityEngine.Random.Range (0, 300.0f);
			workerObject.transform.localPosition = new Vector3 (x,y,0);
			mCharacterList.Add (workerObject.GetComponent<Character> ());
		}
	}

	//通常時の初期化処理
	private void InitNormal () {

		//アイドルを生成
		for (int i = 0; i < mStageData.IdleCount; i++) {
			GameObject idlePrefab = Resources.Load ("Model/Idle/Idle_" + areaParams.stageId) as GameObject;
			GenerateIdle (idlePrefab);
		}

		//ファンを生成
		for (int i = 0; i < mStageData.IdleCount * 3; i++) {
			int rand = UnityEngine.Random.Range (1, 14);
			GameObject fanPrefab = Resources.Load ("Model/Fan/Fan_" + rand) as GameObject;
			GameObject fanObject = Instantiate (fanPrefab) as GameObject;
			float x = UnityEngine.Random.Range (-250.0f, 250.0f);
			float y = UnityEngine.Random.Range (-230.0f, -180.0f);
			fanObject.transform.parent = transform.parent;
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
		mTotalGenerateCoinPower = areaParams.GetGeneratePower (mStageData.IdleCount);
		generateCoinPowerLabel.text = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);

		//アイドルの数をセット
		idleCountLabel.text = "×" + mStageData.IdleCount;

		//アイドルの画像をセット
		idleSprite.spriteName = "idle_normal_" + areaParams.stageId;
		UISpriteData spriteData = idleSprite.GetAtlasSprite ();
		idleSprite.SetDimensions (spriteData.width, spriteData.height);
	}

	//アイドルを生成
	private GameObject GenerateIdle (GameObject idlePrefab) {
		GameObject idleObject = Instantiate (idlePrefab) as GameObject;
		idleObject.transform.parent = transform.parent;
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
		float constructionTimeSeconds = areaParams.constructionTimeMInutes * 60;
	//	float constructionTimeSeconds = (areaParams.constructionTimeMInutes * 60) / 10;
		float timeSpanSeconds = TimeSpanCalculator.CalcFromNow (mStageData.UpdatedDate);
		mTimeSeconds = constructionTimeSeconds - timeSpanSeconds;
	}

	//サボるまでの時間をセット
	private void SetUntilSleepTime () {
		float untilSleepTimeSeconds = areaParams.GetUntilSleepTimeMinutes (mStageData.IdleCount) * 60;
	//	float untilSleepTimeSeconds = (areaParams.GetUntilSleepTimeMinutes (mStageData.IdleCount) * 60) / 10;
		float timeSpanSeconds = TimeSpanCalculator.CalcFromNow (mStageData.UpdatedDate);
		mTimeSeconds = untilSleepTimeSeconds - timeSpanSeconds;
	}
}
