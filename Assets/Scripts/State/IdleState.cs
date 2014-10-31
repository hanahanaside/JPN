using UnityEngine;
using System.Collections;

public interface IdleState {

	void Move (GameObject gameobject);

	void Stop();

	void DirectionUp ();

	void DirectionDown();

	void DirectionLeft();

	void DirectionRight();

	float FlightDuration();
}
