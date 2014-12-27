using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	public GameObject labelObject;
	private int mIndex;

	void Start(){


	}

	public void ButtonClicked(){
		Entity_tutorial entityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		labelObject.GetComponent<TypewriterEffect> ().ResetToBeginning();
		labelObject.GetComponent<UILabel> ().text = entityTutorial.param[mIndex].message;
		mIndex++;
	}
}
