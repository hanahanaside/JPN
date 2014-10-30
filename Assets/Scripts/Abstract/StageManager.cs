using UnityEngine;
using System.Collections;

public abstract class StageManager <T> : MonoSingleton<T> where T : StageManager<T>{

	public GameObject fanPrefab;
}
