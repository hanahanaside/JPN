using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDataDao : Dao<StageDataDao> {

	public List<StageData> GetStageDataList () {
		List<StageData> stageDataList = new List<StageData> ();
		for (int i = 1; i <= 47; i++) {
			StageData stageData = new StageData ();
			stageData.Id = i;
			stageDataList.Add (stageData);
		}
		return stageDataList;
	}
}
