using UnityEngine;
using System.Collections;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }

	void Start(){
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
	}

	public void OnBackButtonClicked () {
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}
		
	public void OnInsert1Clicked(){
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (40);
		stage.IdleCount = 1;
		stage.UpdatedDate = System.DateTime.Now.ToString ();
		dao.UpdateRecord (stage);
	}

	public void OnInsert2Clicked(){
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (2);
		stage.IdleCount = 2;
		stage.UpdatedDate = System.DateTime.Now.ToString ();
		dao.UpdateRecord (stage);
	}

}
