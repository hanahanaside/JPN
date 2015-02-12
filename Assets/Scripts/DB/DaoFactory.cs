using UnityEngine;
using System.Collections;

public static class DaoFactory {

	private static StageDao sStageDao;
	private static GenerateCoinPowerDao sGenerateCoinPowerDao;
	private static UntilSleepTimeDao sUntilSleepTimeDao;
	private static ConstructionTimeDao sConstructionTimeDao;

	public static StageDao CreateStageDao () {
		if (sStageDao == null) {
			sStageDao = new StageDbDao ();
		}
		return  sStageDao;
	}

	public static GenerateCoinPowerDao CreateGenerateCoinPowerDao () {
		if (sGenerateCoinPowerDao == null) {
			sGenerateCoinPowerDao = new GenerateCoinPowerDbDao ();
		}
		return sGenerateCoinPowerDao;
	}

	public static UntilSleepTimeDao CreateUntilSleepTimeDao () {
		if (sUntilSleepTimeDao == null) {
			sUntilSleepTimeDao = new UntilSleepTimeDbDao ();
		}
		return sUntilSleepTimeDao;
	}

	public static ConstructionTimeDao CreateConstructionTimeDao () {
		if (sConstructionTimeDao == null) {
			sConstructionTimeDao = new ConstructionTimeDbDao ();
		}
		return sConstructionTimeDao;
	}
}
