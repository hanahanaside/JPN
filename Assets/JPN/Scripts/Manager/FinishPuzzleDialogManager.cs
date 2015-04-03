using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FinishPuzzleDialogManager : MonoSingleton<FinishPuzzleDialogManager> {

	public static event Action BackToStageEvent;
	public static event Action<int> RetryEvent;

	public GameObject retryButtonObject;
	public GameObject resultPuzzlePrefab;
	public UILabel costLabel;
	public UIGrid buttonGrid;
	public UIGrid resultGrid;

	private int mCost;

	private GameObject mDialogObject;

	public override void OnInitialize () {
		mDialogObject = transform.FindChild ("Dialog").gameObject;
	}

	void CompleteDismissEvent () {
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	void CompleteShowEvent () {
		AdManager.instance.ShowRectangleAd ();
	}

	public void OnBackToStageClicked () {
		AdManager.instance.HideRectangleAd ();
		BackToStageEvent ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnRetryClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if (PlayerDataKeeper.instance.CoinCount < mCost) {
			OKDialog.instance.OnOKButtonClicked = () => {
				BuyCoinDialog.instance.Show ();
			};
			//OKダイアログが見えなくなるのでレクタングルを非表示にする
			AdManager.instance.HideRectangleAd ();
			OKDialog.instance.Show ("コインが不足しています");
			return;
		}
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		RetryEvent (0);
		AdManager.instance.HideRectangleAd ();
	}

	public void Show () {
		mDialogObject.SetActive (true);
		mCost = AreaCostCaluculator.instance.CalcCost (ScoutStageManager.SelectedAreaId - 1);
		buttonGrid.Reposition ();
		costLabel.text = "" + mCost;
		foreach (string itemTag in PuzzleSceneManager.instance.GetItemTagList) {
			GameObject resultPuzzleObject = Instantiate (resultPuzzlePrefab) as GameObject;
			resultGrid.AddChild (resultPuzzleObject.transform);
			resultPuzzleObject.transform.localScale = new Vector3 (1, 1, 1);
			UISprite sprite = resultPuzzleObject.GetComponent<UISprite> ();
			sprite.spriteName = "puzzle_" + itemTag;
		}
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}

	public void Dismiss () {
		iTweenEvent.GetEvent (gameObject, "DismissEvent").Play ();
		List<Transform> childList = resultGrid.GetChildList ();
		foreach (Transform child in childList) {
			Destroy (child.gameObject);
		}
	}
}
