using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoutStageManager : MonoSingleton<ScoutStageManager> {

	public Transform[] areaPositionArray;
	public GameObject dartsObject;
	public GameObject planeObject;
	public GameObject fadeOutSpriteObject;
	public GameObject goScoutButtonObject;
	public UILabel costLabel;
	private int mCost;
	private List<Fan> mFanList = new List<Fan> ();
	private GameObject mContainerObject;

	public static bool FlagScouting{ get; set; }

	public static int SelectedAreaId{ get; set; }

	void OnEnable () {
		AreaPanelManager.instance.OnAreaClickedEvent += OnAreaClickedEvent; 
	}

	void OnDisable () {
		AreaPanelManager.instance.OnAreaClickedEvent -= OnAreaClickedEvent;
	}

	public override void OnInitialize () {
		if (SelectedAreaId == 0) {
			SelectedAreaId = 1;
		}
		dartsObject.transform.localPosition = areaPositionArray [SelectedAreaId - 1].localPosition;
		dartsObject.SetActive (true);
		mCost = AreaCostCaluculator.instance.CalcCost (SelectedAreaId - 1);
		mContainerObject = transform.FindChild ("Container").gameObject;
		costLabel.text = "" + mCost;

		for (int i = 0; i < 20; i++) {
			int rand = UnityEngine.Random.Range (1, 14);
			GameObject fanPrefab = Resources.Load ("Model/Fan/Fan_" + rand) as GameObject;
			GameObject fanObject = Instantiate (fanPrefab) as GameObject;
			float x = UnityEngine.Random.Range (-250.0f, 250.0f);
			float y = UnityEngine.Random.Range (-220.0f, -160.0f);
			fanObject.transform.parent = mContainerObject.transform;
			fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			fanObject.transform.localPosition = new Vector3 (x, y, 0);
			mFanList.Add (fanObject.GetComponent<Fan> ());
			fanObject.GetComponent<Fan> ().Init ();
		}
	}

	void Update(){
		float distance = Vector3.Distance (transform.position,HanautaCamera.instance.Postision);
		if(distance > 2){
			HideFanObject ();
		}else {
			ShowFanObject ();
		}
	}

	private void HideFanObject(){
		mContainerObject.SetActive (false);
	}

	private void ShowFanObject(){
		mContainerObject.SetActive (true);
	}

	void OnAreaClickedEvent (int areaIndexNumber) {
		mCost = AreaCostCaluculator.instance.CalcCost (areaIndexNumber);
		SelectedAreaId = areaIndexNumber + 1;
		dartsObject.transform.localPosition = areaPositionArray [areaIndexNumber].localPosition;
		dartsObject.SetActive (true);
		costLabel.text = "" + mCost; 
	}

	void OnPlaneEventCompleted () {
		fadeOutSpriteObject.SetActive (true);
	}
		
	public void OnFadeOutFinished () {
		SuruPassAdBanner.instance.Hide ();
		Application.LoadLevel ("Puzzle");
	}

	public void OnAreaButtonClicked () {
		AreaPanelManager.instance.ShowAreaPanel ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	public void OnGoScoutButtonClicked () {
		if(PlayerDataKeeper.instance.CoinCount < mCost){
			FenceManager.instance.ShowFence ();
			OKDialog.instance.OnOKButtonClicked = () => {
				BuyCoinDialog.instance.Show();
			};
			OKDialog.instance.Show ("コインが不足しています");
			return;
		}
		goScoutButtonObject.SetActive (false);
		FlagScouting = true;
		PlayerDataKeeper.instance.DecreaseCoinCount (mCost);
		iTweenEvent.GetEvent (planeObject, "moveOut").Play ();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Plane);
	}

	public void PlayMoveInPlaneAnimation () {
		iTweenEvent.GetEvent (planeObject, "moveIn").Play ();
	}

	public void StartLive(){
		foreach(Fan fan in mFanList){
			fan.StartLive ();
		}
	}

	public void FinishLive(){
		foreach(Fan fan in mFanList){
			fan.FinishLive ();
		}
	}

}
