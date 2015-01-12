using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzleTable : MonoBehaviour {

	public static event Action<GameObject[]> CreatedPuzzleTableEvent;
	public static event Action FinishedAnswerCheckEvent;

	private List<Transform> mChildList;
	public GameObject[] blankPuzzleArray;
	public string[] puzzleTagArray;

	//パズルテーブルを作成する
	public void CreateTable () {
		UITable table = GetComponent<UITable> ();
		mChildList = table.children;
		//ターゲットのリストを作成する
		string[] targetTagArray = CreatePuzzleTagArray ();
		//パズルのリストを作成する
		GameObject[] puzzleObjectArray = new GameObject[targetTagArray.Length];
		for (int i = 0; i < puzzleObjectArray.Length; i++) {
			string puzzleTag = targetTagArray [i];
			GameObject puzzlePrefab = Resources.Load ("Puzzle/Puzzle_" + puzzleTag) as GameObject;
			puzzleObjectArray [i] = puzzlePrefab;
		}

		for (int i = 0; i < puzzleObjectArray.Length; i++) {
			GameObject puzzleObject = puzzleObjectArray [i];
			Puzzle puzzle = puzzleObject.GetComponent<Puzzle> ();

			//パズルを設置するインデックスの配列を作成
			int[] puzzleIndexArray = CreateIndexArray (puzzle);

			//nullだったらチケットに変更する
			if (puzzleIndexArray == null) {
				Debug.Log ("nullなのでチケットに変更 " +puzzleObject.tag);
				puzzleObject = Resources.Load<GameObject> ("Puzzle/Puzzle_Ticket");
				puzzleObjectArray [i] = puzzleObject;
				puzzleIndexArray = CreateIndexArray (puzzleObject.GetComponent<Puzzle>());
			}

			//作成した配列にパズルを設置
			AddPuzzle (puzzleIndexArray, puzzleObject);

		}

		//残りの場所にブランクを設置
		AddEmptyPuzzle ();

		//テーブルを整列
		table.Reposition ();

		CreatedPuzzleTableEvent (puzzleObjectArray);
	}

	//答え合わせをする
	public IEnumerator AnswerCheck () {
		yield return new WaitForSeconds (1.0f);
		foreach (Transform childTransform in mChildList) {
			GameObject grandChildObject = childTransform.GetChild (0).gameObject;
			if (!grandChildObject.collider.enabled) {
				continue;
			}
			grandChildObject.collider.enabled = false;
			if (0 <= grandChildObject.tag.IndexOf ("idle")) {
				Puzzle puzzle = grandChildObject.GetComponent<Puzzle> ();
				puzzle.Open ();
				yield return new WaitForSeconds (0.3f);
			}
			if (0 <= grandChildObject.tag.IndexOf ("ticket")) {
				Puzzle puzzle = grandChildObject.GetComponent<Puzzle> ();
				puzzle.Open ();
				yield return new WaitForSeconds (0.3f);
			}
		}
		FinishedAnswerCheckEvent ();
	}
		
	//指定した配列の順番にパズルを設置する
	private void AddPuzzle (int[] indexArray, GameObject puzzlePrefab) {
		for (int i = 0; i < mChildList.Count; i++) {
			Transform child = mChildList [i];
			foreach (int index in indexArray) {
				if (i == index) {
					GameObject	puzzleObject = Instantiate (puzzlePrefab)as GameObject;
					puzzleObject.transform.parent = child;
					puzzleObject.transform.localPosition = new Vector3 (0, 0, 0);
					puzzleObject.transform.localScale = new Vector3 (1, 1, 1);
				}
			}
		}
	}

	//空のパズルを設置
	private void AddEmptyPuzzle () {
		foreach (Transform child in mChildList) {
			if (child.childCount != 0) {
				continue;
			}
			int rand = UnityEngine.Random.Range (0, blankPuzzleArray.Length);
			GameObject	puzzleObject = Instantiate (blankPuzzleArray [rand])as GameObject;
			puzzleObject.transform.parent = child;
			puzzleObject.transform.localPosition = new Vector3 (0, 0, 0);
			puzzleObject.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

	//パズルを配置するインデックスの配列を生成して返す
	private int[] CreateIndexArray (Puzzle puzzle) {
	
		//パズルを設置するインデックスの配列を作成
		int[] puzzleIndexArray = new int[puzzle.rangeArray.Length + 1];

		//10回試して完成しなかったらnullを返す
		for (int i = 0; i < 10; i++) {
			//1つめのパズルを設置する場所をランダムで決定
			int rand = UnityEngine.Random.Range (0, puzzle.firstIndexArray.Length);
			puzzleIndexArray [0] = puzzle.firstIndexArray [rand];

			//2つめ以降のパズルを設置する場所を決定
			for (int j = 1; j < puzzleIndexArray.Length; j++) {
				puzzleIndexArray [j] = puzzleIndexArray [0] + puzzle.rangeArray [j - 1];
			}

			//子供がいなかったら作成を終了
			if (!CheckChildExist (puzzleIndexArray)) {
				return puzzleIndexArray;
			}

		}
		return null;

	}

	//既に子供が存在していたらtrueを返す
	private bool CheckChildExist (int[] indexArray) {
		foreach (int index in indexArray) {
			Transform child = mChildList [index];
			if (child.childCount != 0) {
				return true;
			}
		}
		return false;
	}

	//パズルIDをを返す
	private string[] CreatePuzzleTagArray () {
		string[] targetTagArray = new string[2];
		targetTagArray [0] = GetPuzzleIndex ();
		while (true) {
			targetTagArray [1] = GetPuzzleIndex ();
			if (targetTagArray [0] != targetTagArray [1]) {
				break;
			}
		}
			
		return targetTagArray;
	}

	private string GetPuzzleIndex () {
		int rand = UnityEngine.Random.Range (0, 100);
		if (rand == 99) {
			return puzzleTagArray [6];
		}
		if (rand >= 90) {
			return puzzleTagArray [5];
		}
		if (rand >= 80) {
			return puzzleTagArray [4];
		}
		if (rand >= 60) {
			return puzzleTagArray [3];
		}
		if (rand >= 40) {
			return puzzleTagArray [2];
		}
		if (rand >= 20) {
			return puzzleTagArray [1];
		} 
		return puzzleTagArray [0];
	}
}
