using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDataDao : Singleton<StageDataDao> {

	public List<StageData> GetStageDataList () {
		List<StageData> stageDataList = new List<StageData> ();
		for (int i = 1; i <= 10; i++) {
			StageData stageData = new StageData ();
			stageData.Id = i;
			stageData.IdleCount = i;
			stageData.AreaName = AreaName(i);
			stageDataList.Add (stageData);
		}
		return stageDataList;
	}

	private string AreaName(int id){
		string areaName = "";
		switch(id){
		case 1:
			areaName = "北海道";
			break;
		case 2:
			areaName = "青森";
			break;
		case 3:
			areaName = "岩手";
			break;
		case 4:
			areaName = "宮城";
			break;
		case 5:
			areaName = "秋田";
			break;
		case 6:
			areaName = "山形";
			break;
		case 7:
			areaName = "福島";
			break;
		case 8:
			areaName = "茨城";
			break;
		case 9:
			areaName = "栃木";
			break;
		case 10:
			areaName = "群馬";
			break;
		}
		return areaName;
	}
}
