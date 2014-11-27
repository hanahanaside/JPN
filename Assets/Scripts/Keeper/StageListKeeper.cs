using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageListKeeper : MonoSingleton<StageListKeeper> {

	private List<Stage> mStageDataList;

	public void LoadData(){
		StageDao dao = DaoFactory.CreateStageDao ();
		mStageDataList = dao.SelectAll ();
	}

	public void SaveData(){

	}

	public Stage GetStageData(int index){
		return mStageDataList[index];
	}

	public int ListCount{
		get{
			return mStageDataList.Count;
		}
	}
}
