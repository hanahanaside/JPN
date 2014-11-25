using UnityEngine;
using System.Collections;

public class AreaParams : MonoBehaviour {

	public int areaId;
	public int stageId;
	public float constructionTimeHours;
	public GeneratePowerParams generatePowerParams;
	public UntilSleepParams untilSleepParams;

	public double GetGeneratePower(int idleCount){
		if(idleCount <= 5){
			return generatePowerParams.level_1;
		}
		if(idleCount <= 10){
			return generatePowerParams.level_2;
		}
		if(idleCount <= 15){
			return generatePowerParams.level_3;
		}
		if(idleCount <= 20){
			return generatePowerParams.level_4;
		}
		if(idleCount <= 24){
			return generatePowerParams.level_5;
		}
		return generatePowerParams.level_Max;
	}

	public float GetUntilSleepTime(int idleCount){
		if(idleCount <= 5){
			return untilSleepParams.level_1;
		}
		if(idleCount <= 10){
			return untilSleepParams.level_2;
		}
		if(idleCount <= 15){
			return untilSleepParams.level_3;
		}
		if(idleCount <= 20){
			return untilSleepParams.level_4;
		}
		if(idleCount <= 24){
			return untilSleepParams.level_5;
		}
		return untilSleepParams.level_Max;
	}
}
