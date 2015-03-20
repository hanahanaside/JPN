using UnityEngine;
using System.Collections;

public class AreaParams : MonoBehaviour {

	public int areaId;
	public int stageId;
	public float constructionTimeMInutes;
	public GeneratePowerParams generatePowerParams;
	public UntilSleepParams untilSleepTimeMinutes;

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

	public float GetUntilSleepTimeMinutes(int idleCount){
		if(idleCount <= 5){
			return untilSleepTimeMinutes.level_1;
		}
		if(idleCount <= 10){
			return untilSleepTimeMinutes.level_2;
		}
		if(idleCount <= 15){
			return untilSleepTimeMinutes.level_3;
		}
		if(idleCount <= 20){
			return untilSleepTimeMinutes.level_4;
		}
		if(idleCount <= 24){
			return untilSleepTimeMinutes.level_5;
		}
		return untilSleepTimeMinutes.level_Max;
	}
}
