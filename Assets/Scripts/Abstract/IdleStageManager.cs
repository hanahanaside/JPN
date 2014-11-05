using UnityEngine;
using System.Collections;

public abstract class IdleStageManager<T> : StageManager<T> where T : IdleStageManager<T> {

	public Idle idlePrefab;

	[SerializeField]
	private UILabel untilSleepTimeLabel;
	[SerializeField]
	private UILabel idleCountLabel;
	[SerializeField]
	private UILabel generateSpeedLabel;
	[SerializeField]
	private UISprite idleSprite;
	[SerializeField]
	private float untilSleepTime;
	[SerializeField]
	private GameObject fenceObject;
	[SerializeField]
	private GameObject wakeupButtonObject;

	public abstract void OnWakeupButtonClicked ();

	public bool IsSleeping {
		get;
		set;
	}

	public string IdleSprite{
		set {
			idleSprite.spriteName = value;
		}
	}

	public float UntilSleepTime {
		get{
			return untilSleepTime;
		}
	}

	public string GenerateSpeedLabel{
		set{
			generateSpeedLabel.text = value;
		}
	}

	public string UntilSleepLabel{
		set{
			untilSleepTimeLabel.text = value;
		}
	}

	public void ShowFence(){
		fenceObject.SetActive (true);
	}

	public void HideFence(){
		fenceObject.SetActive (false);
	}

	public void ShowWakeupButton(){
		wakeupButtonObject.SetActive (true);
	}

	public void HideWakeupButton(){
		wakeupButtonObject.SetActive (false);
	}

}