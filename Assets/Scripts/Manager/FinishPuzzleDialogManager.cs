using UnityEngine;
using System.Collections;
using System;

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
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		RetryEvent (0);
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void Show () {
		mDialogObject.SetActive (true);
		mCost = AreaCostCaluculator.instance.CalcCost (ScoutStageManager.SelectedAreaId - 1);
		if (PlayerDataKeeper.instance.CoinCount < mCost) {
			retryButtonObject.SetActive (false);
		} else {
			retryButtonObject.SetActive (true);
		}
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
	}
}
