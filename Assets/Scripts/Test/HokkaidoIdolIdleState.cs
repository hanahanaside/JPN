using UnityEngine;
using System.Collections;

public class HokkaidoIdolIdleState : IdolState {

	private int mMoveCount;
	private HokkaidoIdol mHokkaidoIdol;

	public HokkaidoIdolIdleState (HokkaidoIdol hokkaidoIdol) {
		mHokkaidoIdol = hokkaidoIdol;
		mMoveCount = 5;
	}

	public IdolState Move (GameObject idolObject) {
		iTween.MoveAdd (idolObject, iTween.Hash ("x", 0.1f, "oncomplete", "MoveFinished"));
		mMoveCount--;
		return this;
	}
}
