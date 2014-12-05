using UnityEngine;
using System.Collections;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }
	public UILabel remainingTapCountLabel;
	private int mRemainingTapCount = 10;

	void OnEnable(){
		Puzzle.OpenedPuzzleEvent += OpenedPuzzleEvent;
	}

	void OnDisable(){
		Puzzle.OpenedPuzzleEvent -= OpenedPuzzleEvent;
	}

	void Start(){
		PlayerDataKeeper.instance.Init ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
		remainingTapCountLabel.text = "残りタップ" +mRemainingTapCount +"回";
	}

	void OpenedPuzzleEvent(string puzzleTag){
		mRemainingTapCount--;
		remainingTapCountLabel.text = "残りタップ" +mRemainingTapCount +"回";
	}

	public void OnBackButtonClicked () {
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}
}
