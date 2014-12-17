using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public UILabel label;

	void Start(){

	}

	public void OnButtonClicked(){
		if(label.gameObject.activeSelf){
			label.gameObject.SetActive (false);
			return;
		}else {
			label.gameObject.SetActive (true);
		}
		label.GetComponent<TypewriterEffect> ().ResetToBeginning ();
		label.text = "ああああああああああああ";
	}
}
