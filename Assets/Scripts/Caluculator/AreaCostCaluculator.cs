using UnityEngine;
using System.Collections;

public class AreaCostCaluculator {

	private static AreaCostCaluculator sInstance;
	private static Entity_Area mEntityArea;

	public static AreaCostCaluculator instance {
		get {
			if (sInstance == null) {
				sInstance = new AreaCostCaluculator ();
			}
			if(mEntityArea== null){
				mEntityArea = Resources.Load ("Data/Area") as Entity_Area; //=> Resourcesからデータファイルの読み込み
			}
			return sInstance;
		}
	}

	public int CalcCost(int areaIndex){
		int[] clearedCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		int costStart = mEntityArea.param [areaIndex].cost_start;
		int costAdd = mEntityArea.param [areaIndex].cost_add;
		int clearedCount = clearedCountArray [areaIndex];
		int cost = costStart + (costAdd * clearedCount);
		return cost;
	}
}
