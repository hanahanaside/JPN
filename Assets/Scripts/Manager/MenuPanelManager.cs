using UnityEngine;
using System.Collections;

public class MenuPanelManager : MonoSingleton<MenuPanelManager> {

	public GameObject dialogObject;
	public UIButton seButton;
	public UIButton bgmButton;
	public UIButton firstIdolSleepNotificationButton;
	public UIButton lastIdolSleepNotificationButton;

	void OnEnable () {
		EtceteraManager.mailComposerFinishedEvent += mailComposerFinished;
	}

	void OnDisable () {
		EtceteraManager.mailComposerFinishedEvent -= mailComposerFinished;
	}

	void CompleteDismissEvent () {
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
		dialogObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	void mailComposerFinished (string result) {
		Debug.Log ("mailComposerFinished : " + result);
	}

	public void ShowMenuPanel () {
		dialogObject.SetActive (true);
		//SEボタンのラベルをセット
		if (PrefsManager.instance.SE_ON) {
			ChangeButtonToON (seButton);
		} else {
			ChangeButtonToOFF (seButton);
		}
		//BGMボタンのラベルをセット
		if (PrefsManager.instance.BGM_ON) {
			ChangeButtonToON (bgmButton);
		} else {
			ChangeButtonToOFF (bgmButton);
		}
		//最初のアイドルがサボる通知のラベルをセット
		if (PrefsManager.instance.FirstIdolSleepNotificationON) {
			ChangeButtonToON (firstIdolSleepNotificationButton);
		} else {
			ChangeButtonToOFF (firstIdolSleepNotificationButton);
		}
		//最後のアイドルがサボる通知のラベルをセット
		if (PrefsManager.instance.LastIdolSleepNotificationON) {
			ChangeButtonToON (lastIdolSleepNotificationButton);
		} else {
			ChangeButtonToOFF (lastIdolSleepNotificationButton);
		}
		iTweenEvent.GetEvent (dialogObject, "ShowEvent").Play ();
	}

	public void OnCloseButtonClicked () {
		iTweenEvent.GetEvent (dialogObject, "DismissEvent").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void SEButtonClicked () {
		if (PrefsManager.instance.SE_ON) {
			PrefsManager.instance.SE_ON = false;
			ChangeButtonToOFF (seButton);
		} else {
			PrefsManager.instance.SE_ON = true;
			ChangeButtonToON (seButton);
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void BGMButtonClicked () {
		if (PrefsManager.instance.BGM_ON) {
			PrefsManager.instance.BGM_ON = false;
			ChangeButtonToOFF (bgmButton);
			SoundManager.instance.StopBGM ();
		} else {
			PrefsManager.instance.BGM_ON = true;
			ChangeButtonToON (bgmButton);
			SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void FirstIdolSleepNotificationClicked () {
		if (PrefsManager.instance.FirstIdolSleepNotificationON) {
			PrefsManager.instance.FirstIdolSleepNotificationON = false;
			ChangeButtonToOFF (firstIdolSleepNotificationButton);
		} else {
			PrefsManager.instance.FirstIdolSleepNotificationON = true;
			ChangeButtonToON (firstIdolSleepNotificationButton);
		}
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void  LastIdolSleepNotificationClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if (PrefsManager.instance.LastIdolSleepNotificationON) {
			PrefsManager.instance.LastIdolSleepNotificationON = false;
			ChangeButtonToOFF (lastIdolSleepNotificationButton);
		} else {
			PrefsManager.instance.LastIdolSleepNotificationON = true;
			ChangeButtonToON (lastIdolSleepNotificationButton);
		}
	}

	public void MailButtonClicked () {
		string adress = "app@hnut.co.jp";
		string title = "title";
		string message = "message";
		EtceteraBinding.showMailComposer (adress, title, message, false);
	}

	private void ChangeButtonToON (UIButton button) {
		button.normalSprite = "bt_green";
		UILabel label = button.GetComponentInChildren<UILabel> ();
		label.text = "ON";
	}

	private void ChangeButtonToOFF (UIButton button) {
		button.normalSprite = "bt_red";
		UILabel label = button.GetComponentInChildren<UILabel> ();
		label.text = "OFF";
	}

}
