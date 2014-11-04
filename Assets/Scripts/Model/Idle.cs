using UnityEngine;
using System.Collections;

public abstract class Idle : Character {

	public MovableArea movableArea;
	public IdleParameter normalIdleParam;
	public IdleParameter danceIdleParam;

	public abstract void WakeUp ();

	public abstract void Sleep ();

	public void Move (IdleState idleState) {
		if (characterTransform.localPosition.x > movableArea.limitRight) {
			transform.eulerAngles = new Vector3 (0, 0, 0);
			idleState.Stop ();
			idleState.DirectionLeft ();
		}
		if (characterTransform.localPosition.x < movableArea.limitLeft) {
			transform.eulerAngles = new Vector3 (0, 180.0f, 0);
			idleState.Stop ();
			idleState.DirectionRight ();
		}
		if (characterTransform.localPosition.y > movableArea.limitTop) {
			idleState.Stop ();
			idleState.DirectionDown ();
		}
		if (characterTransform.localPosition.y < movableArea.limitBottom) {
			idleState.Stop ();
			idleState.DirectionUp ();
		}
		idleState.Move (gameObject);
	}
		
}
