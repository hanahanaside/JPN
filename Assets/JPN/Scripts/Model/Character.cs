﻿using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

	public MovableArea movableArea;
	public float moveTimeSeconds;
	public float stopTimeSeconds;
	public float maxSpeedX;
	public float maxSpeedY;
	[HideInInspector]
	public MoveSpeed moveSpeed;

	protected Transform characterTransform;
	protected UISprite uiSprite = null;

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
		float speedX = Random.Range (0, maxSpeedX);
		float speedY = Random.Range (0, maxSpeedY);
		if (moveSpeed.speedX < 0) {
			speedX = -speedX;
		}
		if (moveSpeed.speedY < 0) {
			speedY = -speedY;
		}
		moveSpeed.speedX = speedX;
		moveSpeed.speedY = speedY;
		switch (direction) {
		case Direction.Left:
			characterTransform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
			if (moveSpeed.speedX > 0) {
				moveSpeed.speedX = -moveSpeed.speedX;
			}
			break;
		case Direction.Right:
			characterTransform.localScale = new Vector3 (-0.8f, 0.8f, 0.8f);
			if (moveSpeed.speedX < 0) {
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

	protected UISprite sprite {
		get {
			if (uiSprite == null) {
				uiSprite = transform.FindChild ("Sprite").GetComponent<UISprite> ();
			}
			return uiSprite;
		}
	}
}
