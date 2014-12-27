using UnityEngine;
using System.Collections;

public class TutorialManager : MonoSingleton<TutorialManager> {

	public GameObject natsumotoObject;
	public GameObject scoutButtonObject;
	public GameObject areaButtonObject;
	public GameObject areaDialogObject;
	public GameObject mapObject;
	public GameObject goScoutButtonObject;
	public GameObject planeObject;
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

	public void StartTutorial(){
		PlayerDataKeeper.instance.Init ();
		StageGridManager.instance.CreateStageGrid ();
		StageGridManager.instance.MoveToStage (1);

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
		case 2:
			PlayerDataKeeper.instance.IncreaseCoinCount (2500);
			break;
		case 3:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			scoutButtonObject.SetActive (true);
			return;
		case 4:
			iTweenEvent.GetEvent (natsumotoObject, "HideEvent").Play ();
			areaButtonObject.SetActive (true);
			return;
		}
		mTutorialIndex++;
		UpdateMessage ();
	}

	public void ScoutButtonClicked(){
		StageGridManager.instance.MoveToStage (0);
		natsumotoObject.SetActive (true);
		iTweenEvent.GetEvent (natsumotoObject,"ShowEvent").Play();
		mTutorialIndex++;
		UpdateMessage ();
		scoutButtonObject.SetActive (false);
	}

	public void AreaButtonClicked(){
		areaDialogObject.SetActive (true);
		iTweenEvent.GetEvent (areaDialogObject,"ShowEvent").Play();
		areaButtonObject.SetActive (false);
	}

	public void SugekitaButtonClicked(){
		iTweenEvent.GetEvent (areaDialogObject,"HideEvent").Play();
		mapObject.SetActive (true);
		goScoutButtonObject.SetActive (true);
	}

	public void GoScoutButtonClicked(){
		iTweenEvent.GetEvent (planeObject,"moveOut").Play();
	//	Application.LoadLevel ("Puzzle");
	}

	private void UpdateMessage(){
		typeWriterEffect.ResetToBeginning ();
		tutorialLabel.text = mEntityTutorial.param [mTutorialIndex].message;
	}
}
