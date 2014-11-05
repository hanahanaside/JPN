using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

	protected Transform characterTransform;

	public MovableArea movableArea;
	public float moveSpeedX;
	public float moveSpeedY;

	public abstract void StartDancing();

	public abstract void StopDancing ();

	public void CheckFlip () {
		if (characterTransform.localPosition.x < movableArea.limitLeft) {
			moveSpeedX = -moveSpeedX;
		}
		if (characterTransform.localPosition.x > movableArea.limitRight) {
			moveSpeedX = -moveSpeedX;
		}
		if (characterTransform.localPosition.y < movableArea.limitBottom) {
			moveSpeedY = -moveSpeedY;
		}
		if (characterTransform.localPosition.y > movableArea.limitTop) {
			moveSpeedY = -moveSpeedY;
		}
	}

	public void ChangeDirection () {
		int rand = Random.Range (0, 4);
		switch (rand) {
		case 0:
			break;
		case 1:
			moveSpeedX = -moveSpeedX;
			break;
		case 2:
			moveSpeedY = -moveSpeedY;
			break;
		case 3:
			moveSpeedX = -moveSpeedX;
			moveSpeedY = -moveSpeedY;
			break;
		}
	}
		
	public void Move () {
		characterTransform.Translate (new Vector3 (moveSpeedX, moveSpeedY, 0));
	}

}
