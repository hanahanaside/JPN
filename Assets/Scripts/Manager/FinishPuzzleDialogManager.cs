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

	public void OnBackToStageClicked () {
		BackToStageEvent ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnRetryClicked () {
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if (PlayerDataKeeper.instance.CoinCount < mCost) {
			BuyCoinDialog.instance.Show ();
			return;
		}
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		RetryEvent (0);

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
		foreach(Transform child in childList){
			Destroy (child.gameObject);
		}
	}
}
