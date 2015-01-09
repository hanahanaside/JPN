using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	public GameObject labelObject;
	private int mIndex;

	void Start(){
		Entity_UntilSleepTime entityGenerateCoinPower = Resources.Load<Entity_UntilSleepTime> ("Data/UntilSleepTime");
		Entity_UntilSleepTime.Param param = entityGenerateCoinPower.param[1];
		Debug.Log (""+ param.level_1);
	}

	public void ButtonClicked(){
		Entity_tutorial entityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		labelObject.GetComponent<TypewriterEffect> ().ResetToBeginning();
		labelObject.GetComponent<UILabel> ().text = entityTutorial.param[mIndex].message;
		mIndex++;
	}
}
