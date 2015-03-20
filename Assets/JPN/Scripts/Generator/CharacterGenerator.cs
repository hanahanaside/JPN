using UnityEngine;
using System.Collections;

public class CharacterGenerator : MonoBehaviour {

	//ファンを生成して親クラスを返す
	public Character GenerateFan(){
		int rand = UnityEngine.Random.Range (1, 14);
		GameObject fanPrefab = Resources.Load ("Model/Fan/Fan_" + rand) as GameObject;
		GameObject fanObject = Instantiate (fanPrefab) as GameObject;
		float x = UnityEngine.Random.Range (-250.0f, 250.0f);
		float y = UnityEngine.Random.Range (-230.0f, -180.0f);
		fanObject.transform.parent = transform;
		fanObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		fanObject.transform.localPosition = new Vector3 (x, y, 0);
		fanObject.GetComponent<Fan> ().Init ();
		return fanObject.GetComponent<Character> ();
	}

	//アイドルを生成して親クラスを返す
	public Character GenerateIdol(StageData stageData){
		GameObject idlePrefab = Resources.Load ("Model/Idle/Idle_" + stageData.Id) as GameObject;
		GameObject idleObject = GenerateIdol (idlePrefab);
		return idleObject.GetComponent<Character> ();
	}

	//迷子のアイドルを生成して親クラスを返す
	public Character GenerateLostIdol(int idolId){
		GameObject lostIdlePrefab = Resources.Load ("Model/Idle/Idle_" + idolId) as GameObject;
		GameObject lostIdleObject =	GenerateIdol (lostIdlePrefab);
		BoxCollider boxCollider = lostIdleObject.AddComponent<BoxCollider> ();
		boxCollider.isTrigger = true;
		boxCollider.size = new Vector3 (150, 150, 0);
		GameObject exPrefab = Resources.Load ("GUI/EX") as GameObject;
		GameObject exObject = Instantiate (exPrefab) as GameObject;
		exObject.transform.parent = lostIdleObject.transform;
		exObject.transform.localScale = new Vector3 (1, 1, 1);
		exObject.transform.localPosition = new Vector3 (0, 80, 0);
		return lostIdleObject.GetComponent<Character> ();
	}

	//労働者を生成して親クラスを返す
	public Character GenerateWorker(int workerId){
		GameObject workerPrefab = Resources.Load ("Model/Worker/Worker_" + workerId) as GameObject;
		GameObject workerObject = Instantiate (workerPrefab) as GameObject;
		workerObject.transform.parent = transform;
		workerObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		float x = UnityEngine.Random.Range (-175.0f, 175.0f);
		float y = UnityEngine.Random.Range (0, 300.0f);
		workerObject.transform.localPosition = new Vector3 (x, y, 0);
		workerObject.GetComponent<Worker> ().Init ();
		return workerObject.GetComponent<Character> ();
	}

	private GameObject GenerateIdol (GameObject idlePrefab) {
		GameObject idleObject = Instantiate (idlePrefab) as GameObject;
		idleObject.transform.parent = transform;
		idleObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		float x = UnityEngine.Random.Range (-175.0f, 175.0f);
		float y = UnityEngine.Random.Range (0, 300.0f);
		idleObject.transform.localPosition = new Vector3 (x, y, 0);
		idleObject.GetComponent<Idle> ().Init ();
		return idleObject;
	}

}
