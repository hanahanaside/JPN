using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
public class CheckQuitDialog : MonoSingleton<CheckQuitDialog> {


	private GameObject mDialogObject;

	public override void OnInitialize () {
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	void CompleteDismissEvent () {
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	public void Show () {
		if (mDialogObject.activeSelf) {
			return;
		}
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (true);
		iTweenEvent.GetEvent (mDialogObject, "ShowEvent").Play ();
	}

	public void CancelButtonClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void QuitButtonClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Application.Quit ();
	}

	private void Dismiss () {
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject, "DismissEvent").Play ();
	}
}
#endif