using UnityEngine;
using System.Collections;

public class IdleStageManager<T> : StageManager<T> where T : IdleStageManager<T> {

	public GameObject idlePrefab;

}
