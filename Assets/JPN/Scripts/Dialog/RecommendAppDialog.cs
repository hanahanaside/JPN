using UnityEngine;
using System.Collections;
using MiniJSON;

public class RecommendAppDialog : MonoSingleton<RecommendAppDialog> {
	 
	public UILabel messageLabel;
	private const string PUBLICITIES_URL = "http://push.tt5.us/showed_publicities/1.json";
	private GameObject mDialogObject;
	private RecommendApp mRecommendApp;

	void CompleteDismissEvent () {
		mDialogObject.SetActive (false);
		mDialogObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	public override void OnInitialize () {
		mDialogObject = transform.FindChild ("Dialog").gameObject;
		WWWClient wwwClient = new WWWClient (this, PUBLICITIES_URL);
		wwwClient.OnSuccess = (string response) => {
			mRecommendApp = new RecommendApp ();
			IDictionary jsonObject = (IDictionary)Json.Deserialize (response);
			mRecommendApp.publicityId = (int)((long)jsonObject ["publicity_id"]);
			mRecommendApp.url = (string)jsonObject ["url"];
			mRecommendApp.message = (string)jsonObject ["message"];
		};
		wwwClient.Request ();
	}

	public void Show () {
		if (mRecommendApp == null) {
			return;
		}
		FenceManager.instance.ShowFence ();
		mDialogObject.SetActive (true);
		messageLabel.text = mRecommendApp.message;
		iTweenEvent.GetEvent (mDialogObject, "ShowEvent").Play ();
	}

	public void CancelButtonClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		Dismiss ();
	}

	public void InstallButtonClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		Application.OpenURL (mRecommendApp.url);
	}

	private void Dismiss () {
		FenceManager.instance.HideFence ();
		iTweenEvent.GetEvent (mDialogObject, "DismissEvent").Play ();
	}
}
