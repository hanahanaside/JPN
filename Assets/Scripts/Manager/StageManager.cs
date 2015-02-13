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

	private float mTimeSeconds;
	private double mTotalGenerateCoinPower;
	private State mState = State.Normal;
	private StageData mStageData;
	private List<Character> mCharacterList = new List<Character> ();
	private GameObject mDanceTeamObject;
	private IdolStageContainer mIdolStageContainer;
	private IdolStageCoinGenerator mIdolStageCoinGenerator;
	private CharacterGenerator mCharacterGenerator;

	void OnEnable () {
		Idle.FoundEvent += FoundIdleEvent;
	}

	void OnDisable () {
		Idle.FoundEvent -= FoundIdleEvent;
	}

	public void Init (StageData stageData) {
		mStageData = stageData;
		mIdolStageContainer = GetComponentInChildren<IdolStageContainer> ();
		mIdolStageCoinGenerator = GetComponentInChildren<IdolStageCoinGenerator> ();
		mCharacterGenerator = GetComponentInChildren<CharacterGenerator> ();
		mIdolStageContainer.FindObjects ();

		//工事中でなかったら通常状態へ
		if (mStageData.FlagConstruction == StageData.NOT_CONSTRUCTION) {
			mTimeSeconds = (float)RemainingSleepTimeSec ();
			InitNormal ();
			return;
		}
		//建設完了までの秒数
		double remainingConstructionTimeSec = RemainingConstructionTimeSec ();

		//まだ建設が完了していない場合
		if (remainingConstructionTimeSec > 0) {
			mTimeSeconds = (float)remainingConstructionTimeSec;
			InitConstruction ();
			return;
		}

		//建設完了を貫通した時間
		double overTimeSec = -remainingConstructionTimeSec;

		//建設完了を貫通した時間がサボるまでの時間を超えている場合
		UntilSleepTimeDao untilSleepTimeDao = DaoFactory.CreateUntilSleepTimeDao ();
		int untilSleepTimeSec = untilSleepTimeDao.SelectById (mStageData.Id, mStageData.IdolCount) * 60;
		if (overTimeSec > untilSleepTimeSec) {
			mStageData.FlagConstruction = StageData.NOT_CONSTRUCTION;
			DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
			InitNormal ();
			Sleep ();
			return;
		}

		//建設は完了しているがサボるまでは至っていない場合
		mTimeSeconds = (float)(untilSleepTimeSec - overTimeSec);
		mStageData.FlagConstruction = StageData.NOT_CONSTRUCTION;
		DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
		InitNormal ();
	}

	void Update () {

		//画面内・画面外の処理
		float distance = Vector3.Distance (transform.position, HanautaCamera.instance.Postision);
		if (distance > 2) {
			if (mIdolStageContainer.IsContainerShowing ()) {
				mIdolStageContainer.HideContainer ();
			}
		} else {
			if (!mIdolStageContainer.IsContainerShowing ()) {
				mIdolStageContainer.ShowContainer ();
			}
		}

		switch (mState) {
		case State.Normal:
			//スリープ時間を更新
			mTimeSeconds -= Time.deltaTime;
			mIdolStageContainer.SetUntilSleepLabel ("あと" + TimeConverter.Convert (mTimeSeconds) + "でサボる");
			if (mTimeSeconds < 0) {
				Sleep ();
				return;
			}
			//コイン生成時間を更新
			mIdolStageCoinGenerator.PassTime (Time.deltaTime, mTotalGenerateCoinPower);
			break;
		case State.Live:
			//コイン生成時間を更新(2倍)
			mIdolStageCoinGenerator.PassTime (Time.deltaTime * 2.0f, mTotalGenerateCoinPower);
			if (mStageData.FlagConstruction == StageData.NOT_CONSTRUCTION) {
				return;
			}
			//建設中の場合の処理
			mTimeSeconds -= Time.deltaTime * 2.0f;
			if (mTimeSeconds >= 0) {
				mIdolStageContainer.SetUntilSleepLabel ("あと" + TimeConverter.Convert (mTimeSeconds) + "で完成");
			}
			break;
		case State.Sleep:
			break;
		case State.Construction:
			//建設中の時間を更新
			mTimeSeconds -= Time.deltaTime;
			mIdolStageContainer.SetUntilSleepLabel ("あと" + TimeConverter.Convert (mTimeSeconds) + "で完成");
			if (mTimeSeconds > 0) {
				return;
			}
			//建設完了
			mStageData.FlagConstruction = StageData.NOT_CONSTRUCTION;
			mStageData.UpdatedDate = DateTime.Now.ToString ();
			DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
			mTimeSeconds = GetUntilSleepTimeMin () * 60;
			FinishConstruction ();
			break;
		}
	}

	//建設完了
	private void FinishConstruction () {
		mIdolStageContainer.ChangeBackgroundTexture ("Texture/St_" + mStageData.Id);
		foreach (Character character in mCharacterList) {
			Destroy (character.gameObject);
		}
		mState = State.Normal;
		mCharacterList = new List<Character> ();
		InitNormal ();
		MyLog.LogDebug ("finish construction stage " + mStageData.Id);
	}

	void FoundIdleEvent (Character character) {
		mCharacterList.Remove (character);
	}

	//再開時の処理
	public void Resume () {
		//工事中かをチェック
		if (mStageData.FlagConstruction == StageData.NOT_CONSTRUCTION) {
			float timeSpanSeconds = TimeSpanCalculator.CalcFromNow (PlayerDataKeeper.instance.ExitDate);
			mTimeSeconds -= timeSpanSeconds;
			return;
		} 

		//建設完了までの秒数
		double remainingConstructionTimeSec = RemainingConstructionTimeSec ();

		//まだ建設が完了していない場合
		if (remainingConstructionTimeSec > 0) {
			mTimeSeconds = (float)remainingConstructionTimeSec;
			return;
		}

		//建設完了を貫通した時間
		double overTimeSec = -remainingConstructionTimeSec;

		//建設完了を貫通した時間がサボるまでの時間を超えている場合
		UntilSleepTimeDao untilSleepTimeDao = DaoFactory.CreateUntilSleepTimeDao ();
		int untilSleepTimeSec = untilSleepTimeDao.SelectById (mStageData.Id, mStageData.IdolCount) * 60;
		if (overTimeSec > untilSleepTimeSec) {
			mStageData.FlagConstruction = StageData.NOT_CONSTRUCTION;
			DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
			FinishConstruction ();
			Sleep ();
			return;
		}

		//建設は完了しているがサボるまでは至っていない場合
		mTimeSeconds = (float)(untilSleepTimeSec - overTimeSec);
		mStageData.FlagConstruction = StageData.NOT_CONSTRUCTION;
		DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
		FinishConstruction ();

	}
		
	//喝ボタン押下時の処理
	public void Wakeup () {
		mTimeSeconds = GetUntilSleepTimeMin () * 60;
		mStageData.UpdatedDate = DateTime.Now.ToString ();
		DaoFactory.CreateStageDao ().UpdateRecord (mStageData);
		mState = State.Normal;
		gameObject.tag = "default";
		//画像を変更
		mIdolStageContainer.ChangeIdolSprite ("idle_normal_" + mStageData.Id);
		//コイン生成パワーを算出してセット
		mTotalGenerateCoinPower = GetGenerateCoinPower ();
		mIdolStageContainer.SetGenerateCoinPowerLabel (GameMath.RoundOne (mTotalGenerateCoinPower) + "/分");
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		WakeupEvent ();
		MyLog.LogDebug ("wake up stage " + mStageData.Id);
	}

	//サボりを開始
	private void Sleep () {
		gameObject.tag = "sleep";
		mIdolStageContainer.ShowSleepObjects ();
		mState = State.Sleep;
		mIdolStageContainer.SetSleep (mStageData);
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
		mIdolStageContainer.HideSkipConstructionButton ();
		mIdolStageContainer.SetUntilSleepLabel ("LIVE！！！！！！！！！！！");
		gameObject.tag = "default";
		if (mIdolStageContainer.IsSleepObjectsShowing ()) {
			mIdolStageContainer.HideSleepObjects ();
			//画像を変更
			mIdolStageContainer.ChangeIdolSprite ("idle_normal_" + mStageData.Id);
			WakeupEvent ();
			PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		}
		//ライブ時は2倍
		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
		foreach (Character character in mCharacterList) {
			character.StartLive ();
		}
		if (mStageData.FlagConstruction != StageData.IN_CONSTRUCTION) {
			mDanceTeamObject = Instantiate (danceTeamPrefab)as GameObject;
			mDanceTeamObject.transform.parent = mIdolStageContainer.transform;
			mDanceTeamObject.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
			mDanceTeamObject.transform.localPosition = new Vector3 (20, 10, 0);
			DanceTeamManager danceTeamManager = mDanceTeamObject.GetComponent<DanceTeamManager> ();
			danceTeamManager.StartDancing (mStageData.Id, mStageData.IdolCount);
			mIdolStageContainer.SetGenerateCoinPowerLabel (GameMath.RoundOne (mTotalGenerateCoinPower * 2) + "/分");
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
			mIdolStageContainer.ShowSkipConstructionButton ();
		} else {
			mTimeSeconds = GetUntilSleepTimeMin () * 60;
			mIdolStageContainer.SetGenerateCoinPowerLabel (GameMath.RoundOne (mTotalGenerateCoinPower) + "/分");
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
		for (int i = 0; i < count; i++) {
			Character character = mCharacterGenerator.GenerateIdol (mStageData);
			mCharacterList.Add (character);
		}
		mStageData = DaoFactory.CreateStageDao ().SelectById (mStageData.Id);
		mIdolStageContainer.SetIdolCountLabel (mStageData);
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
		mIdolStageContainer.SetIdolCountLabel (mStageData);
	}

	//今すぐ完成させるボタン押下
	public void SkipConstruction () {
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
		Character character = mCharacterGenerator.GenerateIdol (mStageData);
		mCharacterList.Add (character);
	}

	//工事中の初期化処理
	private void InitConstruction () {
		gameObject.tag = "construction";

		mCharacterList = new List<Character> ();
		mState = State.Construction;
		mIdolStageContainer.SetConstruction (mStageData);

		//労働者を生成
		for (int i = 1; i <= 4; i++) {
			Character character = mCharacterGenerator.GenerateWorker (i);
			mCharacterList.Add (character);
		}
	}

	//建設完了までの秒数を返す
	private double RemainingConstructionTimeSec () {
		ConstructionTimeDao constructionTimeDao = DaoFactory.CreateConstructionTimeDao ();
		int constructionTimeSec = constructionTimeDao.SelectById (mStageData.Id) * 60;
		DateTime dtStartConstruction = DateTime.Parse (mStageData.UpdatedDate);
		DateTime dtNow = DateTime.Parse (DateTime.Now.ToString ());
		TimeSpan ts = dtNow - dtStartConstruction;
		return constructionTimeSec - ts.TotalSeconds;
	}

	//サボるまでの秒数を返す
	private double RemainingSleepTimeSec () {
		DateTime dtNow = DateTime.Now;
		DateTime dtUpdatedDate = DateTime.Parse (mStageData.UpdatedDate);
		TimeSpan ts = dtNow - dtUpdatedDate;
		double untilSleepTimeSec = GetUntilSleepTimeMin () * 60;
		return untilSleepTimeSec - ts.TotalSeconds;
	}

	//通常時の初期化処理
	private void InitNormal () {
		//アイドルを生成
		for (int i = 0; i < mStageData.IdolCount; i++) {
			Character character = mCharacterGenerator.GenerateIdol (mStageData);
			mCharacterList.Add (character);
		}

		//ファンを生成
		for (int i = 0; i < mStageData.IdolCount * 5; i++) {
			Character character = mCharacterGenerator.GenerateFan ();
			mCharacterList.Add (character);
		}
			
		mTotalGenerateCoinPower = GetGenerateCoinPower ();

		mIdolStageContainer.SetNormal (mStageData, mTotalGenerateCoinPower);

		PlayerDataKeeper.instance.IncreaseGenerateCoinPower (mTotalGenerateCoinPower);
	}
		
	//サボるまでの時間をDBから取得
	private int GetUntilSleepTimeMin () {
		UntilSleepTimeDao dao = DaoFactory.CreateUntilSleepTimeDao ();
		return dao.SelectById (mStageData.Id, mStageData.IdolCount);
	}

	//コイン生成パワーをDBから取得
	private double GetGenerateCoinPower () {
		GenerateCoinPowerDao dao = DaoFactory.CreateGenerateCoinPowerDao ();
		return dao.SelectById (mStageData.Id, mStageData.IdolCount);
	}
}
