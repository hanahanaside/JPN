using UnityEngine;
using System.Collections;
using System;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	public Transform UIroot;

	void OnEnable(){
		Referee.FinishGameEvent += FinishPuzzleEvent;
	}

	void OnDisable(){
		Referee.FinishGameEvent -= FinishPuzzleEvent;
	}

	void Start () {
		PlayerDataKeeper.instance.Init ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
		GameObject puzzleContainerPrefab = Resources.Load ("PuzzleContainer/PuzzleContainer") as GameObject;
		GameObject puzzleContainerObject = Instantiate (puzzleContainerPrefab)as GameObject;
		puzzleContainerObject.transform.parent = UIroot;
		puzzleContainerObject.transform.localScale = new Vector3 (1, 1, 1);
		puzzleContainerObject.transform.localPosition = new Vector3 (0,0,0);
	}
						
	//パズルを終了
	void FinishPuzzleEvent(){
		PlayerDataKeeper.instance.SaveData ();
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}
}
