using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

	public MovableArea movableArea;
	public MoveSpeed moveSpeed;
	public float moveTimeSeconds;
	public float stopTimeSeconds;

	protected Transform characterTransform;

	protected enum Direction {
		Left,
		Right,
		Up,
		Down,
	}

	protected enum State {
		Move,
		Stop,
		Live,
		Sleep,
	}

	void Awake () {
		characterTransform = transform;
	}

	public abstract void StartLive ();

	public abstract void FinishLive ();

	public abstract void StartMoving ();

	public abstract void Stop ();

	public abstract void Sleep ();

	public abstract void Wakeup ();

	//移動方向を変更
	protected void ChangeDirection (Direction direction) {
		switch (direction) {
		case Direction.Left:
			characterTransform.localScale = new Vector3 (1,1,1);
			if(moveSpeed.speedX > 0){
				moveSpeed.speedX = -moveSpeed.speedX;
			}
			break;
		case Direction.Right:
			characterTransform.localScale = new Vector3 (-1, 1, 1);
			if(moveSpeed.speedX < 0){
				moveSpeed.speedX = -moveSpeed.speedX;
			}
			break;
		case Direction.Down:
			if (moveSpeed.speedY > 0) {
				moveSpeed.speedY = -moveSpeed.speedY;
			}
			break;
		case Direction.Up:
			if (moveSpeed.speedY < 0) {
				moveSpeed.speedY = -moveSpeed.speedY;
			}
			break;
		}
	}

	//上限の座標に達しているかをチェック
	protected bool CheckLimit () {
		if (characterTransform.localPosition.x < movableArea.limitLeft) {
			return true;
		}
		if (characterTransform.localPosition.x > movableArea.limitRight) {
			return true;
		}
		if (characterTransform.localPosition.y < movableArea.limitBottom) {
			return true;
		}
		if (characterTransform.localPosition.y > movableArea.limitTop) {
			return true;
		}
		return false;
	}

	//進むべき方向をチェックする(どこでも良ければランダム)
	protected Direction CheckDirection () {
		if (characterTransform.localPosition.x < movableArea.limitLeft) {
			return Direction.Right;
		}
		if (characterTransform.localPosition.x > movableArea.limitRight) {
			return Direction.Left;
		}
		if (characterTransform.localPosition.y < movableArea.limitBottom) {
			return Direction.Up;
		}
		if (characterTransform.localPosition.y > movableArea.limitTop) {
			return Direction.Down;
		}
		int rand = Random.Range (0, 4);
		return (Direction)rand;
	}


}
