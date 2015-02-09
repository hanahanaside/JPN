using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StageManager : MonoBehaviour {

	public static event Action SleepEvent;
	public static event Action WakeupEvent;

	public enum State {
		Normal,
		Sleep,
		Live,
		Construction
	}

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
	private Transform mTransform;
	private IdolStageStatus mIdolStageStatus;

	void OnEnable () {
		Idle.FoundEvent += FoundIdleEvent;
	}

	void OnDisable () {
		Idle.FoundEvent -= FoundIdleEvent;
	}

	public void Init (StageData stage) {
		mStageData = stage;
		mTransform = transform;
		mSkipConstructionButtonObject = transform.Find ("Container/SkipConstructionButton").gameObject;
		mContainerObject = transform.FindChild ("Container").gameObject;
		sleepObject = transform.Find ("Container/Sleep").gameObject;
		mIdolStageStatus = transform.Find ("Container/IdolStageStatus").GetComponent<IdolStageStatus> ();
		backGroundTexture = GetComponentInChildren<UITexture> ();
		mIdolStageStatus.Init ();
		//工事中かをチェック
		if (mStageData.State == StageData.StateType.Construction) {
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
			mIdolStageStatus.UntilSleepLabel = "あと" + TimeConverter.Convert (mTimeSeconds) + "でサボる";
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
			if (mStageData.State == StageData.StateType.Construction) {
				mTimeSeconds -= Time.deltaTime * 2.0f;
				if (mTimeSeconds >= 0) {
					mIdolStageStatus.UntilSleepLabel = "あと" + TimeConverter.Convert (mTimeSeconds) + "で完成";
				}
			}
			break;
		case State.Sleep:
			break;
		case State.Construction:
			//建設中の時間を更新
			mTimeSeconds -= Time.deltaTime;
			mIdolStageStatus.UntilSleepLabel = "あと" + TimeConverter.Convert (mTimeSeconds) + "で完成";
			if (mTimeSeconds > 0) {
				return;
			}
			//建設完了
			backGroundTexture.mainTexture = Resources.Load ("Texture/St_" + mStageData.Id) as Texture;
			foreach (Character character in mCharacterList) {
				Destroy (character.gameObject);
			}
			mState = State.Normal;
			mStageData.State = StageData.StateType.Normal;
			mStageData.UpdatedDate = DateTime.Now.ToString ();
			DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
			mCharacterList = new List<Character> ();
			InitNormal ();
			MyLog.LogDebug ("finish construction stage " + mStageData.Id);
			break;
		}

		//画面内・画面外の処理
		float distance = Vector3.Distance (mTransform.position, HanautaCamera.instance.Postision);
		if (distance > 2) {
			if (mContainerObject.activeSelf) {
				mContainerObject.SetActive (false);
			}
		} else {
			if (!mContainerObject.activeSelf) {
				mContainerObject.SetActive (true);
			}
		}
	}

	void FoundIdleEvent (Character character) {
		mCharacterList.Remove (character);
	}

	//再開時の処理
	public void Resume () {
		//工事中かをチェック
		if (mStageData.State == StageData.StateType.Construction) {
			SetConstructionTime ();
		} else {
			//サボるまでの時間をセット(テストで10分の1)
			SetUntilSleepTime ();
		}
	}

	//ステージデータを返す
	public StageData Stage {
		get {
			return mStageData;
		}
	}

	//ステートを返す
	public State GetState {
		get {
			return mState;
		}
	}

	//サボるか工事完了までの時間を返す
	public float UntilTime {
		get {
			return mTimeSeconds;
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
		mIdolStageStatus.IdolSpriteName = "idle_normal_" + mStageData.Id;
		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = GetGenerateCoinPower ();
		mIdolStageStatus.GenerateCoinPowerLabel = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		WakeupEvent ();
		//アイコン広告を非表示にする
		AdManager.instance.HideIconAd ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Katsu);
		MyLog.LogDebug ("wake up stage " + mStageData.Id);
	}

	//サボりを開始
	private void Sleep () {
		gameObject.tag = "sleep";
		sleepObject.SetActive (true);
		mState = State.Sleep;
		//コイン生成パワーをセット
		mIdolStageStatus.GenerateCoinPowerLabel = "0/分";
		//サボるまでの時間をセット
		mIdolStageStatus.UntilSleepLabel = "サボり中";
		//画像を変更
		mIdolStageStatus.IdolSpriteName = "idle_sleep_" + mStageData.Id;

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
		mSkipConstructionButtonObject.SetActive (false);
		mIdolStageStatus.UntilSleepLabel = "LIVE！！！！！！！！！！！";
		gameObject.tag = "default";
		if (sleepObject.activeSelf) {
			sleepObject.SetActive (false);
			//画像を変更
			mIdolStageStatus.IdolSpriteName = "idle_normal_" + mStageData.Id;
			WakeupEvent ();
			PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		//ライブ時は2倍
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		foreach (Character character in mCharacterList) {
			character.StartLive ();
		}
		if (mStageData.State != StageData.StateType.Construction) {
			mDanceTeamObject = Instantiate (danceTeamPrefab)as GameObject;
			mDanceTeamObject.transform.parent = mContainerObject.transform;
			mDanceTeamObject.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
			mDanceTeamObject.transform.localPosition = new Vector3 (20, 10, 0);
			DanceTeamManager danceTeamManager = mDanceTeamObject.GetComponent<DanceTeamManager> ();
			danceTeamManager.StartDancing (mStageData.Id, mStageData.IdleCount);
			mIdolStageStatus.GenerateCoinPowerLabel = GameMath.RoundOne (mTotalGenerateCoinPower * 2) + "/分";
		} 
	}

	//ライブを終了
	public void FinishLive () { 
		//ライブ中で終了した場合は倍になっている数字を元に戻す
		if (mState == State.Live) {
			PlayerDataKeeper.instance.DecreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		if (mStageData.State == StageData.StateType.Construction) {
			mState = State.Construction;
			mSkipConstructionButtonObject.SetActive (true);
		} else {
			mTimeSeconds = GetUntilSleepTime () * 60;
			mIdolStageStatus.GenerateCoinPowerLabel = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
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

	//アイドル発見時に追加する処理
	public void AddIdle (int count) {
		Debug.Log ("Add " + count);
		GameObject idlePrefab = Resources.Load ("Model/Idle/Idle_" + mStageData.Id) as GameObject; 
		for (int i = 0; i < count; i++) {
			GenerateIdle (idlePrefab);
		}
		mStageData = DaoFactory.CreateStageDao ().SelectById (mStageData.Id);
		SetIdolCount ();
		switch (mState) {
		case State.Sleep:
			foreach (Character character in mCharacterList) {
				character.Sleep ();
			}
			break;
		case State.Live:
			foreach (Character character in mCharacterList) {
				character.StartLive ();
			}
			break;
		}
	}

	public void RemoveIdle (int count) {
		for (int i = 0; i < count; i++) {
			Character character = mCharacterList [0];
			mCharacterList.Remove (character);
			Destroy (character.gameObject);
		}
		mStageData = DaoFactory.CreateStageDao ().SelectById (mStageData.Id);
		SetIdolCount ();
	}

	//今すぐ完成させるボタン押下
	public void SkipConstructionClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		//20分で1枚
		//小数点以下を切り上げ
		int ticketCount = (int)(Math.Ceiling (mTimeSeconds / (20 * 60)));
		if (ticketCount <= 0) {
			ticketCount = 1;
		}
		SkipConstructionDialog.instance.Show (ticketCount);
		SkipConstructionDialog.instance.positiveButtonClicked = () => {
			if (PlayerDataKeeper.instance.TicketCount < ticketCount) {
				BuyTicketDialog.instance.Show ();
				return;
			}
			mTimeSeconds = 0;
			PlayerDataKeeper.instance.DecreaseTicketCount (ticketCount);
		};
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
		exObject.transform.localScale = new Vector3 (1, 1, 1);
		exObject.transform.localPosition = new Vector3 (0, 80, 0);
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
		mIdolStageStatus.IdolSpriteName = "idle_normal_" + mStageData.Id;

		//アイドルの数をセット
		SetIdolCount ();

		//エリア名をセット
		mIdolStageStatus.AreaNameLabel = "建設中";

		//コイン生成パワーをセット
		mIdolStageStatus.GenerateCoinPowerLabel = "0/分";

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
		for (int i = 0; i < mStageData.IdleCount; i++) {
			GenerateIdle (idlePrefab);
		}

		//ファンを生成
		for (int i = 0; i < mStageData.IdleCount * 5; i++) {
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
		mIdolStageStatus.AreaNameLabel = mStageData.AreaName;

		//サボるまでの時間をセット(テストで10分の1)
		SetUntilSleepTime ();

		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = GetGenerateCoinPower ();
		mIdolStageStatus.GenerateCoinPowerLabel = GameMath.RoundOne (mTotalGenerateCoinPower) + "/分";
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);

		//アイドルの数をセット
		SetIdolCount ();

		//今すぐ完成させるボタンを非表示
		mSkipConstructionButtonObject.SetActive (false);

		//背景をセット
		backGroundTexture.mainTexture = Resources.Load ("Texture/St_" + mStageData.Id) as Texture;

		//アイドルの画像をセット
		mIdolStageStatus.IdolSpriteName = "idle_normal_" + mStageData.Id;
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
		return dao.SelectById (mStageData.Id, mStageData.IdleCount);
	}

	//コイン生成パワーをDBから取得
	private double GetGenerateCoinPower () {
		GenerateCoinPowerDao dao = DaoFactory.CreateGenerateCoinPowerDao ();
		return dao.SelectById (mStageData.Id, mStageData.IdleCount);
	}

	//アイドルの人数をセット
	private void SetIdolCount () {
		if (mStageData.IdleCount >= 25) {
			mIdolStageStatus.IdolCountLabel = "MAX";
		} else {
			mIdolStageStatus.IdolCountLabel = "×" + mStageData.IdleCount;
		}
	}
}
