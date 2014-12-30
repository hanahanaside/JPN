using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PuzzleTableTutorial : MonoBehaviour {

	public static event Action<GameObject[]> CreatedPuzzleTableEvent;
	public static event Action FinishedAnswerCheckEvent;

	private List<Transform> mChildList;
	public GameObject[] blankPuzzleArray;

	//パズルテーブルを作成する
	public void CreateTable () {
		UITable table = GetComponent<UITable> ();
		mChildList = table.children;
		//生成するパズルの種類の数を決める
		int[] targetIdArray = new int[2];
		targetIdArray [0] = 1;
		targetIdArray [1] = 2;
		GameObject[]	puzzleObjectArray = new GameObject[targetIdArray.Length];
		for (int i = 0; i < puzzleObjectArray.Length; i++) {
			int puzzleId = targetIdArray [i];
			GameObject puzzlePrefab = Resources.Load ("Puzzle/Puzzle_" + puzzleId) as GameObject;
			puzzleObjectArray [i] = puzzlePrefab;
		}

		foreach (GameObject puzzleObject in puzzleObjectArray) {
			Puzzle puzzle = puzzleObject.GetComponent<Puzzle> ();

			//パズルを設置するインデックスの配列を作成
			int[] puzzleIndexArray = CreateIndexArray (puzzle);

			//作成した配列にパズルを設置
			AddPuzzle (puzzleIndexArray, puzzleObject);

		}

		//残りの場所にブランクを設置
		AddEmptyPuzzle ();

		//テーブルを整列
		table.Reposition ();

	}

	//答え合わせをする
	public IEnumerator AnswerCheck () {
		yield return new WaitForSeconds (1.0f);
		foreach (Transform childTransform in mChildList) {
			GameObject grandChildObject = childTransform.GetChild (0).gameObject;
			if(!grandChildObject.collider.enabled){
				continue;
			}
			grandChildObject.collider.enabled = false;
			if (0 <= grandChildObject.tag.IndexOf ("idle")) {
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
			GameObject	puzzleObject = Instantiate (blankPuzzleArray [0])as GameObject;
			puzzleObject.transform.parent = child;
			puzzleObject.transform.localPosition = new Vector3 (0, 0, 0);
			puzzleObject.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

	//パズルを配置するインデックスの配列を生成して返す
	private int[] CreateIndexArray (Puzzle puzzle) {

		//パズルを設置するインデックスの配列を作成
		int[] puzzleIndexArray = new int[puzzle.rangeArray.Length + 1];

		while (true) {
			//1つめのパズルを設置する場所をランダムで決定
			int rand = UnityEngine.Random.Range (0, puzzle.firstIndexArray.Length - 1);
			puzzleIndexArray [0] = puzzle.firstIndexArray [rand];

			//2つめ以降のパズルを設置する場所を決定
			for (int i = 1; i < puzzleIndexArray.Length; i++) {
				puzzleIndexArray [i] = puzzleIndexArray [0] + puzzle.rangeArray [i - 1];
			}

			//子供がいなかったら作成を終了
			if (!CheckChildExist (puzzleIndexArray)) {
				break;
			}

		}

		return puzzleIndexArray;
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
}
