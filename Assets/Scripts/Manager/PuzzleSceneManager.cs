using UnityEngine;
using System.Collections;

public class PuzzleSceneManager : MonoSingleton<PuzzleSceneManager> {

	public bool FlagBackButtonClicked{ get; set; }
	private static int id = 0;

	void Start(){
		PlayerDataKeeper.instance.Init ();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Puzzle);
		Debug.Log ("num " + ScoutStageManager.AreaIndexNumber);
	}

	public void OnBackButtonClicked () {
		FlagBackButtonClicked = true;
		Application.LoadLevel ("Main");
	}
		
	public void OnInsert1Clicked(){
		id++;
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (id);
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
