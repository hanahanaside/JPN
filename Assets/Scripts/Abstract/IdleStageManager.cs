using UnityEngine;
using System.Collections;

public abstract class IdleStageManager<T> : StageManager<T> where T : IdleStageManager<T> {

	public Idle idlePrefab;

}