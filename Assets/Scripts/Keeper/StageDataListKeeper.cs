using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDataListKeeper : MonoSingleton<StageDataListKeeper> {

	private List<StageData> mStageDataList;

	public void LoadData(){
		mStageDataList = StageDataDao.instance.GetStageDataList ();
	}

	public void SaveData(){

	}

	public StageData GetStageData(int index){
		return mStageDataList[index];
	}

	public int ListCount{
		get{
			return mStageDataList.Count;
		}
	}
}
