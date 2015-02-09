using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratedCoinCalculator {

	public static double CalcWhileSleeping(){
		StageDao dao = DaoFactory.CreateStageDao ();
		List<StageData> stageDataList = dao.SelectAll ();
		double generatedCoin = 0;
		return generatedCoin;
	}

	//中断中に稼いだコインを計算して返す
//	private double CalcSleepTimeCoin () {
//		DateTime dtNow = DateTime.Now;
//		DateTime dtExit = DateTime.Parse (PlayerDataKeeper.instance.ExitDate);
//		TimeSpan ts = dtNow - dtExit;
//		MyLog.LogDebug ("ts " + ts.TotalSeconds);
//		double addCoin = (PlayerDataKeeper.instance.SavedGenerateCoinPower / 60.0) * ts.TotalSeconds;
//		float remainingLiveTimeSeconds = GetRemainingLiveTimeSeconds ();
//		if (remainingLiveTimeSeconds > 0) {
//			addCoin = addCoin * 2;
//		}
//		MyLog.LogDebug ("addCoin " + addCoin);
//		return addCoin;
//	}
}
