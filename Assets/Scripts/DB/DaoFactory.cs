using UnityEngine;
using System.Collections;

public static class DaoFactory  {

	public static StageDao CreateStageDao(){
		return  new StageDbDao ();
	}

	public static GenerateCoinPowerDao CreateGenerateCoinPowerDao(){
		return new GenerateCoinPowerDbDao ();
	}
}
