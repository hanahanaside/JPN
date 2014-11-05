using UnityEngine;
using System.Collections;

public abstract class Idle : Character {

	public MovableArea movableArea;
	public IdleParams normalIdleParam;
	public IdleParams danceIdleParam;
	private UISprite mSprite;
	private Transform mTransform;
	private Rigidbody2D mRigidbody2D;

	public abstract void WakeUp ();

	public abstract void Sleep ();

	void Awake(){
		mSprite = GetComponentInChildren<UISprite> ();
		mTransform = transform;
		mRigidbody2D = GetComponent<Rigidbody2D> ();
	}

	public void Move (IdleState idleState) {
		if (mTransform.localPosition.x > movableArea.limitRight) {
			transform.eulerAngles = new Vector3 (0, 0, 0);
			idleState.Stop ();
			idleState.DirectionLeft ();
		}
		if (mTransform.localPosition.x < movableArea.limitLeft) {
			transform.eulerAngles = new Vector3 (0, 180.0f, 0);
			idleState.Stop ();
			idleState.DirectionRight ();
		}
		if (mTransform.localPosition.y > movableArea.limitTop) {
			idleState.Stop ();
			idleState.DirectionDown ();
		}
		if (mTransform.localPosition.y < movableArea.limitBottom) {
			idleState.Stop ();
			idleState.DirectionUp ();
		}
		idleState.Move (gameObject);
	}
		
	public void SetSprite(string spriteName){
		mSprite.spriteName = spriteName;
	}

	public bool IsKinematic {
		set{
			mRigidbody2D.isKinematic = value;
		}
	}
}
