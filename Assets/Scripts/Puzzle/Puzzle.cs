using UnityEngine;
using System.Collections;
using System;

public class Puzzle : MonoBehaviour {

	public static event Action<string> OpenedPuzzleEvent;
	private UIButton mButton;

	void Start () {
		mButton = GetComponent<UIButton> ();
		int rand = UnityEngine.Random.Range (1, 5);
		mButton.normalSprite = "puzzle_base_" + rand;
	}

	void OnClick () {
		collider.enabled = false;
		mButton.normalSprite = tag;
		switch(tag){
		case "puzzle_coin_1":
			PlayerDataKeeper.instance.IncreaseCoinCount (1);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_coin_5":
			PlayerDataKeeper.instance.IncreaseCoinCount (5);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_coin_10":
			PlayerDataKeeper.instance.IncreaseCoinCount (10);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_coin_100":
			PlayerDataKeeper.instance.IncreaseCoinCount (100);
			SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.GetCoin);
			break;
		case "puzzle_ticket":
			PlayerDataKeeper.instance.IncreaseTicketCount (1);
			break;
		}
		OpenedPuzzleEvent (tag);
	}
}
