using UnityEngine;
using System.Collections;

public interface State  {

	void MoveAround (Rigidbody2D rididbody2D);

	void Dance();

}
