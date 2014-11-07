using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDataDao : Singleton<StageDataDao> {

	public List<StageData> GetStageDataList () {
		List<StageData> stageDataList = new List<StageData> ();
		for (int i = 1; i <= 1; i++) {
			StageData stageData = new StageData ();
			stageData.Id = i;
			stageData.IdleCount = 2;
			stageData.AreaName = "北海道";
			stageDataList.Add (stageData);
		}
		return stageDataList;
	}
}
