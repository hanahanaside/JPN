using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	public GameObject labelObject;
	private int mIndex;

	void Start(){
		for(int i =0;i <10;i++){
			Debug.Log ("i " +i);
			if(i == 5){
				return;
			}
		}
	}

	public void ButtonClicked(){
		Entity_tutorial entityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		labelObject.GetComponent<TypewriterEffect> ().ResetToBeginning();
		labelObject.GetComponent<UILabel> ().text = entityTutorial.param[mIndex].message;
		mIndex++;
	}
}
