using UnityEngine;
using System.Collections;

public class ReviewDialog : MonoSingleton<ReviewDialog> {

	private GameObject mDialogObject;

	void CompleteDismissEvent(){
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public override void OnInitialize(){
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	public void Show(){
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (mDialogObject,"ShowEvent").Play();
	}

	public void CancelButtonClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void DontShowButtonClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		PrefsManager.instance.IsReviewed = true;
		Dismiss ();
	}

	public void GoReviewButtonClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		PrefsManager.instance.IsReviewed = true;
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (false);
		#if UNITY_IPHONE
		Application.OpenURL ("https://itunes.apple.com/us/app/aidorupurojekuto-aidoru-yu/id955378244?l=ja&ls=1&mt=8");
		#endif
		#if UNITY_ANDROID

		#endif
	}

	private void Dismiss(){
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject,"DismissEvent").Play();
	}
}
