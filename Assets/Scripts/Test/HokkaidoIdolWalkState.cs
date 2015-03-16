using UnityEngine;
using System.Collections;

public class HokkaidoIdolWalkState : IdolState {

	private int mMoveCount;

	public HokkaidoIdolWalkState (HokkaidoIdol hokkaidoIdol) {
		mMoveCount = 5;
	}

	public IdolState Move (GameObject idolObject) {
		iTween.MoveAdd (idolObject, iTween.Hash ("x", 0.1f, "oncomplete", "MoveFinished"));
		mMoveCount--;
		return this;
	}
}
