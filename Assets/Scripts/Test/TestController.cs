using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	public GameObject labelObject;
	private int mIndex;

	void Start(){
		Entity_ConstructionTime entityConstructionTime = Resources.Load<Entity_ConstructionTime> ("Data/ConstructionTime");
		Entity_ConstructionTime.Param param = entityConstructionTime.param[0];
		Debug.Log (""+ param.time);
	}

	public void ButtonClicked(){
		Entity_tutorial entityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		labelObject.GetComponent<TypewriterEffect> ().ResetToBeginning();
		labelObject.GetComponent<UILabel> ().text = entityTutorial.param[mIndex].message;
		mIndex++;
	}
}
