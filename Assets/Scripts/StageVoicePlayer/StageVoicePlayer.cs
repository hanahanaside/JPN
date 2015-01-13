using UnityEngine;
using System.Collections;

public class StageVoicePlayer : MonoSingleton<StageVoicePlayer> {

	public UIGrid stageGrid;
	private int mVoiceIndex;

	public override void OnInitialize () {
		UICenterOnChild uiCenterOnChild = stageGrid.GetComponent<UICenterOnChild> ();
		uiCenterOnChild.onCenter += OnCenterCallBack;
	}

	void OnCenterCallBack (GameObject centeredObject) {
		CancelInvoke ();
		if(centeredObject.tag == "sleep" || centeredObject.tag == "construction"){
			return;
		}
		int index = stageGrid.GetIndex (centeredObject.transform);
		if (index >= 2) {
			mVoiceIndex = index - 2;
			float rand = UnityEngine.Random.Range (0f, 3.0f);
			Invoke ("PlayVoice", rand);
		}
	}

	private void PlayVoice () {
		CharacterVoiceManager.instance.PlayVoice (mVoiceIndex);
	}
}
