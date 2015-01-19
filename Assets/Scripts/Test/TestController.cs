using UnityEngine;
using System.Collections;


public class TestController : TestParent {

	public UILabel label;
	private int mIndex;

	enum Days{Sun = 5,Mon,Tue};

	void Start(){
		Entity_Area entityArea = Resources.Load ("Data/Area") as Entity_Area;
		Entity_Area.Param param = entityArea.param [0];
	//	param.area_name = "aaaa";
		Debug.Log (param.area_name);
	}

	public void ButtonClicked(){
		Entity_Area entityArea = Resources.Load ("Data/Area") as Entity_Area;
		Entity_Area.Param param = entityArea.param [0];
		Debug.Log (param.area_name);
	}

	public void FinishedTypeWriter(){
		Debug.Log ("finished");
	}
}
