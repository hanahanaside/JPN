using UnityEngine;
using System.Collections;

public class UntilSleepTimeDbDao : UntilSleepTimeDao {

	public int SelectById(int stageId,int idolCount){
		Entity_UntilSleepTime entityUntilSleepTime = Resources.Load<Entity_UntilSleepTime> ("Data/UntilSleepTime");
		Entity_UntilSleepTime.Param param = entityUntilSleepTime.param[stageId -1];
		if(idolCount <= 5){
			return param.level_1;
		}
		if(idolCount <= 10){
			return param.level_2;
		}
		if(idolCount <= 15){
			return param.level_3;
		}
		if(idolCount <= 20){
			return param.level_4;
		}
		if(idolCount <= 24){
			return param.level_5;
		}
		return param.max;
	}
}
