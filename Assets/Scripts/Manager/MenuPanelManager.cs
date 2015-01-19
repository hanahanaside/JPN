using UnityEngine;
using System.Collections;

public class MenuPanelManager : MonoSingleton<MenuPanelManager> {

	public GameObject dialogObject;
	public UIButton seButton;
	public UIButton bgmButton;
	public UIButton notificationButton;

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
		dialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public void ShowMenuPanel(){
		dialogObject.SetActive (true);
		//SEボタンのラベルをセット
		if(PrefsManager.instance.SE_ON){
			ChangeButtonToON (seButton);
		}else {
			ChangeButtonToOFF (seButton);
		}
		//BGMボタンのラベルをセット
		if(PrefsManager.instance.BGM_ON){
			ChangeButtonToON (bgmButton);
		}else {
			ChangeButtonToOFF (bgmButton);
		}
		//通知のラベルをセット
		if(PrefsManager.instance.NotificationON){
			ChangeButtonToON (notificationButton);
		}else {
			ChangeButtonToOFF (notificationButton);
		}
		iTweenEvent.GetEvent (dialogObject,"ShowEvent").Play();
	}

	public void OnCloseButtonClicked(){
		iTweenEvent.GetEvent (dialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void SEButtonClicked(){
		if(PrefsManager.instance.SE_ON){
			PrefsManager.instance.SE_ON = false;
			ChangeButtonToOFF (seButton);
		}else {
			PrefsManager.instance.SE_ON = true;
			ChangeButtonToON (seButton);
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void BGMButtonClicked(){
		if(PrefsManager.instance.BGM_ON){
			PrefsManager.instance.BGM_ON = false;
			ChangeButtonToOFF (bgmButton);
			SoundManager.instance.StopBGM ();
		}else {
			PrefsManager.instance.BGM_ON = true;
			ChangeButtonToON (bgmButton);
			SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void NotificationClicked(){
		if(PrefsManager.instance.NotificationON){
			PrefsManager.instance.NotificationON = false;
			ChangeButtonToOFF (notificationButton);
		}else {
			PrefsManager.instance.NotificationON = true;
			ChangeButtonToON (notificationButton);
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	} 

	public void MailButtonClicked(){
		Debug.Log ("lllllll");
	}

	private void ChangeButtonToON(UIButton button){
		button.normalSprite = "cell_green";
		UILabel label = button.GetComponentInChildren<UILabel> ();
		label.text = "ON";
	}

	private void ChangeButtonToOFF(UIButton button){
		button.normalSprite = "cell_red";
		UILabel label = button.GetComponentInChildren<UILabel> ();
		label.text = "OFF";
	}

}
