using UnityEngine;
using System.Collections;

public class IdolStageState : MonoBehaviour {

	public enum State {
		Normal,
		Sleep,
		Live,
		Construction
	}

	private State mState = State.Normal;

	public State state {
		get {
			return mState;
		}
		set {
			mState = value;
		}
	}
}
