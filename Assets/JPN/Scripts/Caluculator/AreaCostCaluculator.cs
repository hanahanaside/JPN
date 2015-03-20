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
			if (mEntityArea == null) {
				mEntityArea = Resources.Load ("Data/Area") as Entity_Area; //=> Resourcesからデータファイルの読み込み
			}
			return sInstance;
		}
	}

	public int CalcCost (int areaIndex) {
		Entity_Area.Param param = mEntityArea.param [areaIndex];
		int[] clearedCountArray = PrefsManager.instance.ClearedPuzzleCountArray;
		int costStart = param.cost_start;
		int costAdd = param.cost_add;
		int clearedCount = clearedCountArray [areaIndex];
		int cost = costStart + (costAdd * clearedCount);
		if (cost > param.cost_end) {
			cost = param.cost_end;
		}
		return cost;
	}
}
