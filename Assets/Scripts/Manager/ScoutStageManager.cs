using UnityEngine;
using System.Collections;

public class ScoutStageManager : MonoSingleton<ScoutStageManager> {

	public Transform[] areaPositionArray;
	public GameObject dartsObject;
	public GameObject planeObject;
	public GameObject fadeOutSpriteObject;
	public GameObject goScoutButtonObject;
	public UILabel costLabel;

	public static bool FlagScouting{ get; set; }

	public static int SelectedAreaId{ get; set; }

	private static double Cost{ get; set; }


	void OnEnable () {
		AreaPanelManager.instance.OnAreaClickedEvent += OnAreaClickedEvent; 
	}

	void OnDisable () {
		AreaPanelManager.instance.OnAreaClickedEvent -= OnAreaClickedEvent;
	}

	void Awake () {
		if (SelectedAreaId != 0) {
			dartsObject.transform.localPosition = areaPositionArray [SelectedAreaId - 1].localPosition;
			dartsObject.SetActive (true);
		}
		if (Cost != 0) {
			costLabel.text = "" + Cost;
		}
	}


	void OnAreaClickedEvent (int areaIndexNumber, int cost) {
		Cost = cost;
		SelectedAreaId = areaIndexNumber + 1;
		dartsObject.transform.localPosition = areaPositionArray [areaIndexNumber].localPosition;
		dartsObject.SetActive (true);
		costLabel.text = "" + cost; 
	}

	void OnPlaneEventCompleted () {
		fadeOutSpriteObject.SetActive (true);
	}

	public void StartLive () {

	}

	public void OnFadeOutFinished () {
		Application.LoadLevel ("Puzzle");
	}

	public void OnAreaButtonClicked () {
		dartsObject.SetActive (false);
		AreaPanelManager.instance.ShowAreaPanel ();
	}

	public void OnGoScoutButtonClicked () {
		if (!dartsObject.activeSelf) {
			return;
		}
		goScoutButtonObject.SetActive (false);
		FlagScouting = true;
		PlayerDataKeeper.instance.DecreaseCoinCount (Cost);
		PlayerDataKeeper.instance.SaveData ();
		iTweenEvent.GetEvent (planeObject, "moveOut").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Plane);
	}

	public void PlayMoveInPlaneAnimation () {
		iTweenEvent.GetEvent (planeObject, "moveIn").Play ();
	}
}
