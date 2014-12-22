using UnityEngine;
using System.Collections;

public class SleepTimeCoinDialogManager : MonoSingleton<SleepTimeCoinDialogManager> {

	public UILabel coinCountLabel;
	public GameObject dialogObject;

	public void Show(double coinCount){
		dialogObject.SetActive (true);
		iTweenEvent.GetEvent (dialogObject,"ShowEvent").Play();
		coinCountLabel.text = "" + GameMath.RoundZero (coinCount);
	}

	public void OKClicked(){
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
	}
}
