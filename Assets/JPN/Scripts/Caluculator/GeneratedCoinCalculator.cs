using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GeneratedCoinCalculator {

	public static double CalcWhileSleeping () {
		double totalGeneratedCoin = 0;
		StageDao stageDao = DaoFactory.CreateStageDao ();
		List<StageData> stageDataList = stageDao.SelectAll ();
		double liveTimeSec = LiveTimeSec ();
		//中断した秒数
		double sleepTimeSec = SleepTimeSec ();
		foreach (StageData stageData in stageDataList) {
			//中断時のサボるまでの秒数
			double untilSleepTimeSec = UntilSleepTimeSecWhenPause (stageData);
			//中断時に既にサボっていた場合は0円
			if (untilSleepTimeSec <= 0) {
				continue;
			}
			//建設中→中断時に稼いだ金額を足す
			if (stageData.FlagConstruction == StageData.IN_CONSTRUCTION) {
				double toFinishConstructionTimeSec = ToFinishConstructionTimeSec (stageData);
				totalGeneratedCoin += GeneratedCoinInConstruction (toFinishConstructionTimeSec, stageData, sleepTimeSec);
			}
			//通常時→中断時に稼いだ金額を足す
			else {
				totalGeneratedCoin += GeneratedCoinInNormal (untilSleepTimeSec, stageData, sleepTimeSec, liveTimeSec);
			}
		}
		return totalGeneratedCoin;
	}

	private static double SleepTimeSec () {
		DateTime dtNow = DateTime.Now;
		DateTime dtExit = DateTime.Parse (PlayerDataKeeper.instance.ExitDate);
		TimeSpan ts = dtNow - dtExit;
		Debug.Log ("sleep time seconds = " + ts.TotalSeconds);
		return ts.TotalSeconds;
	}

	//DBからコイン生成パワーを返す
	private static double GenerateCoinPowerMin (StageData stageData) {
		GenerateCoinPowerDao dao = DaoFactory.CreateGenerateCoinPowerDao ();
		double generateCoinPower = dao.SelectById (stageData.Id, stageData.IdolCount);
		return generateCoinPower;
	}

	//DBからサボるまでの分数を返す
	private static int UntilSleepTimeMin (StageData stageData) {
		UntilSleepTimeDao dao = DaoFactory.CreateUntilSleepTimeDao ();
		int untilSleepTimeMin = dao.SelectById (stageData.Id, stageData.IdolCount);
		return untilSleepTimeMin;
	}

	//DBから建設完了までの分数を返す
	private static int ToFinishConstructionTimeMin (StageData stageData) {
		ConstructionTimeDao dao = DaoFactory.CreateConstructionTimeDao ();
		int toFinishConstructionTimeMin = dao.SelectById (stageData.Id);
		return toFinishConstructionTimeMin;
	}
		
	//中断時のサボるまでの秒数を返す
	private static double UntilSleepTimeSecWhenPause (StageData stageData) {
		//ステージごとのサボるまでの秒数
		double untilSleepTimeSec = UntilSleepTimeMin (stageData) * 60;
		DateTime dtUpdateDate = DateTime.Parse (stageData.UpdatedDate);
		DateTime dtExit = DateTime.Parse (PlayerDataKeeper.instance.ExitDate);
		TimeSpan ts = dtExit - dtUpdateDate;
		return untilSleepTimeSec - ts.TotalSeconds;
	}

	//中断時の建設完了までの秒数を返す
	private static double ToFinishConstructionTimeSec (StageData stageData) {
		double toFinishConstructionTimeSec = ToFinishConstructionTimeMin (stageData) * 60;
		DateTime dtUpdateDate = DateTime.Parse (stageData.UpdatedDate);
		DateTime dtExit = DateTime.Parse (PlayerDataKeeper.instance.ExitDate);
		TimeSpan ts = dtExit - dtUpdateDate;
		return toFinishConstructionTimeSec - ts.TotalSeconds;
	}

	//中断中、何秒間ライブがあったかを返す
	private static double LiveTimeSec () {
		LiveData liveData = PrefsManager.instance.Read<LiveData> (PrefsManager.Kies.LiveData);
		//ライブが始まっていなければ0を返す
		if (liveData.time <= 0) {
			return 0;
		}
		DateTime dtExit = DateTime.Parse (PlayerDataKeeper.instance.ExitDate);
		DateTime dtStartLive = DateTime.Parse (liveData.startDate);
		TimeSpan ts = dtExit - dtStartLive;
		//中断時の残りライブ時間
		double liveTimeSec = liveData.time - ts.TotalSeconds;
		//中断時間が残りライブ時間を上回っていれば残りライブ時間全てを返す
		if (liveTimeSec - SleepTimeSec() < 0) {
			return liveTimeSec;
		} 
		//スリープした時間そのものがライブ時間
		else {
			return SleepTimeSec();
		}
	}

	//ノーマル状態で中断した時に稼いだ金額を計算して返す
	private static double GeneratedCoinInNormal (double untilSleepTimeSec, StageData stageData, double sleepTimeSec, double liveTimeSec) {
		double generatedCoin = 0;
		//稼ぐ力
		double generateCoinPowerSec = (GenerateCoinPowerMin (stageData)) / 60;
		//中断時のサボるまでの秒数 or 中断した秒数が0になるまで足し続ける
		while (true) {
			if (untilSleepTimeSec <= 0) {
				break;
			}
			if (sleepTimeSec <= 0) {
				break;
			}
			//ライブ中は2倍
			if (liveTimeSec > 0) {
				generatedCoin += generateCoinPowerSec * 2;
			} else {
				generatedCoin += generateCoinPowerSec;
			}
			untilSleepTimeSec--;
			sleepTimeSec--;
			liveTimeSec--;
		}
		return generatedCoin;
	}

	//建設中の状態で中断した時に稼いだ金額を計算して返す
	private static double GeneratedCoinInConstruction (double toFinishConstructionTimeSec, StageData stageData, double sleepTimeSec) {
		Debug.Log ("建設までの時間は" + toFinishConstructionTimeSec);
		double generatedCoin = 0;
		//稼ぐ力
		double generateCoinPowerSec = (GenerateCoinPowerMin (stageData)) / 60;
		while (true) {
			if (sleepTimeSec <= 0) {
				break;
			}
			//スリープした時間が建設までの時間を上回った時点から加算を開始する
			if (toFinishConstructionTimeSec <= 0) {
				generatedCoin += generateCoinPowerSec;
			}
			sleepTimeSec--;
			toFinishConstructionTimeSec--;
		}
		Debug.Log ("建設中に稼いだ金額は " + generatedCoin);
		return generatedCoin;
	}
}
