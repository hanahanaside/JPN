using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour {

	enum Direction{
		Left,
		Right,
		Up,
		Down,
	}

	enum State {
		Move,
		Stop,
		Live,
	}

	public MovableArea movableArea;
	public MoveSpeed moveSpeed;
	public float moveTimeSeconds;
	public float stopTimeSeconds;

	private GameObject mSpriteObject;
	private iTweenEvent mRotateEvent;
	private iTweenEvent mJumpEvent;
	private State mState;
	private Transform mTtransform;
	private float mTime;

	void Start () {
		mTtransform = transform;
		mSpriteObject = mTtransform.FindChild ("Sprite").gameObject;
		mRotateEvent = iTweenEvent.GetEvent (mSpriteObject,"RotateEvent");
		mJumpEvent = iTweenEvent.GetEvent (gameObject,"JumpEvent");
		StartMoving ();
	}

	void Update () {
		mTime -= Time.deltaTime;
		switch (mState) {
		case State.Move:
			mTtransform.Translate (new Vector3 (moveSpeed.speedX, moveSpeed.speedY, 0));
			if(CheckLimit()){
				Debug.Log ("limit");
				Stop ();
			}
			if(mTime < 0){
				Stop ();
			}
			break;
		case State.Stop:
			if(mTime < 0){
				Debug.Log ("restart");
				StartMoving ();
			}
			break;
		case State.Live:
			break;
		}
	}

	public void StartLive(){
		mState = State.Live;
		mRotateEvent.Stop ();
		mSpriteObject.transform.localEulerAngles = new Vector3 (0,0,0);
		mJumpEvent.Play ();
	}

	public void FinishLive(){

	}

	private void ChangeDirection (Direction direction) {
		switch (direction) {
		case Direction.Left:
			mTtransform.eulerAngles = new Vector3 (0, 0, 0);
			break;
		case Direction.Right:
			mTtransform.eulerAngles = new Vector3 (0, -180, 0);
			break;
		case Direction.Down:
			moveSpeed.speedY = -moveSpeed.speedY;
			break;
		case Direction.Up:
			moveSpeed.speedY = -moveSpeed.speedY;
			break;
		}
	}

	private bool CheckLimit () {
		if (mTtransform.localPosition.x < movableArea.limitLeft) {
			return true;
		}
		if (mTtransform.localPosition.x > movableArea.limitRight) {
			return true;
		}
		if (mTtransform.localPosition.y < movableArea.limitBottom) {
			return true;
		}
		if (mTtransform.localPosition.y > movableArea.limitTop) {
			return true;
		}
		return false;
	}

	private Direction CheckDirection(){
		if (mTtransform.localPosition.x < movableArea.limitLeft) {
			return Direction.Right;
		}
		if (mTtransform.localPosition.x > movableArea.limitRight) {
			return Direction.Left;
		}
		if (mTtransform.localPosition.y < movableArea.limitBottom) {
			return Direction.Up;
		}
		if (mTtransform.localPosition.y > movableArea.limitTop) {
			return Direction.Down;
		}
		int rand = Random.Range (0, 4);
		Debug.Log ("rand = " + rand );
		return (Direction)rand;
	}

	private void Stop(){
		Debug.Log ("stop");
		mSpriteObject.transform.localEulerAngles = new Vector3 (0,0,0);
		mState = State.Stop;
		mTime = stopTimeSeconds;
		mRotateEvent.Stop ();
	}

	private void StartMoving(){
		mRotateEvent.Play ();
		mState = State.Move;
		mTime = moveTimeSeconds;
		ChangeDirection (CheckDirection());
	}

}
