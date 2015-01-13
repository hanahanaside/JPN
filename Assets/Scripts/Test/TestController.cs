using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	public GameObject labelObject;
	private int mIndex;

	enum Days{Sun = 5,Mon,Tue};

	void Start(){
		Debug.Log ("" + (int)Days.Sun);
	}

	public void ButtonClicked(){
		Entity_tutorial entityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		labelObject.GetComponent<TypewriterEffect> ().ResetToBeginning();
		labelObject.GetComponent<UILabel> ().text = entityTutorial.param[mIndex].message;
		mIndex++;
	}
}
