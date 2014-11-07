using UnityEngine;
using System.Collections;

public class Idle : MonoBehaviour {

	enum State {
		Move,
		Stop,
		Live,
		Sleep}

	;

	enum Direction {
		Left,
		Right,
		Up,
		Down
	}

	public MovableArea movableArea;
	public MoveSpeed moveSpeed;
	public float moveTime;
	public float stopTime;
	public GameObject spriteObject;

	private Transform mTransform;
	private float mTime;
	private State mState = State.Move;
	private iTweenEvent mJumpEvent;
	private iTweenEvent mScaleEvent;
	private bool jump;

	void Start () {
		mTransform = transform;
		mTime = moveTime;
		ChangeDirection (CheckDirection());
		mJumpEvent = iTweenEvent.GetEvent (gameObject, "JumpEvent");
		mScaleEvent = iTweenEvent.GetEvent (gameObject,"ScaleEvent");
		mJumpEvent.Play ();
		mScaleEvent.Play ();
	}

	void Update () {
		switch (mState) {
		case State.Move:
			mTime -= Time.deltaTime;
			mTransform.Translate (new Vector3 (moveSpeed.speedX, moveSpeed.speedY, 0));
			break;
		case State.Stop:
			mTime -= Time.deltaTime;
			//動きを再開
			if (mTime < 0) {
				mState = State.Move;
				mTime = moveTime;
				ChangeDirection (CheckDirection());
				mJumpEvent.Play ();
				mScaleEvent.Play ();
			}
			break;
		case State.Live:
			mTransform.Translate (new Vector3 (moveSpeed.speedX, moveSpeed.speedY, 0));
			break;
		case State.Sleep:
			break;
		}
	}

	private void ChangeDirection (Direction direction) {
		switch (direction) {
		case Direction.Left:
			mTransform.eulerAngles = new Vector3 (0, 0, 0);
			break;
		case Direction.Right:
			mTransform.eulerAngles = new Vector3 (0, -180, 0);
			break;
		case Direction.Down:
			moveSpeed.speedY = -moveSpeed.speedY;
			break;
		case Direction.Up:
			moveSpeed.speedY = -moveSpeed.speedY;
			break;
		}
	}

	private void Stop () {
		mState = State.Stop;
		mTime = stopTime;
		mJumpEvent.Stop ();
		mScaleEvent.Stop ();
		mTransform.localScale = new Vector3 (1f,1f,1f);
	}

	void OnCompleteJumpEvent () {
		jump = !jump;
		if (mState != State.Move) {
			return;
		}
		if(jump){
			return;
		}
		if (mTransform.localPosition.x < movableArea.limitLeft) {
			Stop ();
		}
		if (mTransform.localPosition.x > movableArea.limitRight) {
			Stop ();
		}
		if (mTransform.localPosition.y < movableArea.limitBottom) {
			Stop ();
		}
		if (mTransform.localPosition.y > movableArea.limitTop) {
			if(moveSpeed.speedY > 0){
				Stop ();
			}
		}
		if (mTime < 0) {
			Stop ();
		}
	}

	private Direction CheckDirection(){
		if (mTransform.localPosition.x < movableArea.limitLeft) {
			Debug.Log ("right");
			return Direction.Right;
		}
		if (mTransform.localPosition.x > movableArea.limitRight) {
			Debug.Log ("left");
			return Direction.Left;
		}
		if (mTransform.localPosition.y < movableArea.limitBottom) {
			Debug.Log ("up");
			return Direction.Up;
		}
		if (mTransform.localPosition.y > movableArea.limitTop) {
			Debug.Log ("down");
			return Direction.Down;
		}
		int rand = Random.Range (0,4);
		MyLog.LogDebug ("random");
		return (Direction)rand;
	}

	public  void Sleep () {
		mState = State.Sleep;
	}

	public void Wakeup () {
		mState = State.Move;
	}

	public void StartDancing () {
		mState = State.Live;
	}

	public void StopDancing () {
		mState = State.Move;
	}
}