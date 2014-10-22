using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDataListKeeper : MonoSingleton<StageDataListKeeper> {

	private List<StageData> mStageDataList;

	public void Init(){
		mStageDataList = StageDataDao.instance.GetStageDataList ();
	}

	public StageData GetStageData(int index){
		return mStageDataList[index];
	}
}
