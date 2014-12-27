using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainTutorialManager : MonoSingleton<MainTutorialManager> {

	public GameObject natsumotoObject;
	public GameObject scoutArrowObject;
	public GameObject areaArrowObject;
	public GameObject areaDialogObject;
	public GameObject dartsObject;
	public GameObject goScoutArrowObject;
	public GameObject planeObject;
	public GameObject fadeOutSpriteObject;
	public UICenterOnChild centerOnChild;
	public UIGrid grid;
	private UILabel tutorialLabel;
	private TypewriterEffect typeWriterEffect;
	private Entity_tutorial mEntityTutorial;

	private int mTutorialIndex = 0;

	void CompleteShowEvent(){
		typeWriterEffect.ResetToBeginning ();
		tutorialLabel.text = mEntityTutorial.param [mTutorialIndex].message;
	}

	void CompleteHideEvent(){
		natsumotoObject.transform.localPosition = new Vector3 (0,0,0);
		natsumotoObject.SetActive (false);
	}

	void CompleteHideAreaDialogEvent(){
		areaDialogObject.SetActive (false);
	}

	void OnPlaneEventCompleted(){
		fadeOutSpriteObject.SetActive (true);
	}

	public void OnFadeOutFinished () {
		Application.LoadLevel ("Puzzle");
	}

	void Start(){
		List<Transform> childList = grid.GetChildList ();
		centerOnChild.CenterOn (childList[1]);
		PlayerDataKeeper.instance.Init ();
		mEntityTutorial = Resources.Load<Entity_tutorial> ("Data/tutorial");
		tutorialLabel = natsumotoObject.transform.FindChild ("Label").GetComponent<UILabel>();
		typeWriterEffect = natsumotoObject.transform.FindChild ("Label").GetComponent<TypewriterEffect>();
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		SoundManager.instance.PlayBGM (SoundManager.BGM_CHANNEL.Main);
	}

	public void OKButtonClicked(){
		Debug.Log ("index " +mTutorialIndex);
		switch(mTutorialIndex){
		case 0:
			mTutorialIndex++;
			UpdateMessage ();
			break;
		case 1:
			mTutorialIndex++;
			UpdateMessage ();
			break;
		case 2:
			PlayerDataKeeper.instance.IncreaseCoinCount (2500);
			mTutorialIndex++;
			UpdateMessage ();
			break;
		case 3:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			scoutArrowObject.SetActive (true);
			break;
		case 4:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			areaArrowObject.SetActive (true);
			break;
		case 5:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			areaDialogObject.SetActive (true);
			iTweenEvent.GetEvent (areaDialogObject,"ShowEvent").Play();
			break;
		case 6:
			mTutorialIndex++;
			UpdateMessage ();
			break;
		case 7:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			dartsObject.SetActive (true);
			goScoutArrowObject.SetActive (true);
			break;
		}
	}

	public void ScoutButtonClicked(){
		if(mTutorialIndex != 3){
			return;
		}

		List<Transform> childList = grid.GetChildList ();
		centerOnChild.CenterOn (childList[0]);
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		mTutorialIndex++;
		UpdateMessage ();
		scoutArrowObject.SetActive (false);
	}

	public void AreaButtonClicked(){
		if(mTutorialIndex != 4){
			return;
		}
		areaArrowObject.SetActive (false);
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		mTutorialIndex++;
		UpdateMessage ();
	}

	public void SugekitaButtonClicked(){
		iTweenEvent.GetEvent (areaDialogObject,"HideEvent").Play();
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		mTutorialIndex++;
		UpdateMessage ();
	}

	public void GoScoutButtonClicked(){
		if(mTutorialIndex != 7){
			return;
		}
		iTweenEvent.GetEvent (planeObject,"moveOut").Play();
	}

	private void UpdateMessage(){
		typeWriterEffect.ResetToBeginning ();
		tutorialLabel.text = mEntityTutorial.param [mTutorialIndex].message;
	}
}
