using UnityEngine;
using System.Collections;

public class SkipConstructionDialogTutorial : MonoSingleton<SkipConstructionDialogTutorial> {

	public delegate void PositiveButtonClickedDelegate();
	public PositiveButtonClickedDelegate positiveButtonClicked;
	public UILabel messageLabel;
	public UILabel countLabel;
	private GameObject mDialogObject;

	void CompleteDismissEvent(){
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public override void OnInitialize(){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	public void Show(int ticketCount){
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (mDialogObject,"ShowEvent").Play();
		messageLabel.text = "チケット" + ticketCount + "枚で完成させますか？";
		countLabel.text = "×" + ticketCount + "で";
	}

	public void PositiveButtonClicked(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		positiveButtonClicked ();
	}
}
