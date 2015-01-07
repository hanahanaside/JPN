using UnityEngine;
using System.Collections;

public class SleepTimeCoinDialogManager : MonoSingleton<SleepTimeCoinDialogManager> {

	public UILabel coinCountLabel;
	public GameObject dialogObject;

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1,1,1);
	}

	public void Show(double coinCount){
		dialogObject.SetActive (true);
		iTweenEvent.GetEvent (dialogObject,"ShowEvent").Play();
		coinCountLabel.text = "あなたがいない間に\n" + GameMath.RoundZero (coinCount) + "コイン\n稼ぎました";
	}

	public void OKClicked(){
		iTweenEvent.GetEvent (dialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}
}
