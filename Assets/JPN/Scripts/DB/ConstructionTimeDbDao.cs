using UnityEngine;
using System.Collections;

public class ConstructionTimeDbDao : ConstructionTimeDao {

	public int SelectById(int stageId){
		Entity_ConstructionTime entityConstructionTime = Resources.Load<Entity_ConstructionTime> ("Data/ConstructionTime");
		Entity_ConstructionTime.Param param = entityConstructionTime.param[stageId -1];
		return param.time;
	}
}
