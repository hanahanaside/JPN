using UnityEngine;
using System.Collections;

public class OKDialog : MonoSingleton<OKDialog> {

	public delegate void okButtonClickedDelegate();
	public UILabel titleLabel;
	public GameObject dialogObject;
	private okButtonClickedDelegate mOKButtonClicked;

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
		dialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public void Show(string title){
		dialogObject.SetActive (true);
		titleLabel.text = title;
		iTweenEvent.GetEvent (dialogObject,"ShowEvent").Play();
	}

	public void OKButtonClicked(){
		iTweenEvent.GetEvent (dialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(mOKButtonClicked != null){
			mOKButtonClicked();
		}
	}

	public okButtonClickedDelegate OnOKButtonClicked{
		set{
			mOKButtonClicked = value;
		}
	}
}
