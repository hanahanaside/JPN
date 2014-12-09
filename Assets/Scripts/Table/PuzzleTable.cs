using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzleTable : MonoBehaviour {

	public static event Action<GameObject[]> CreatedPuzzleTableEvent;

	private List<Transform> mChildList;
	public GameObject[] puzzleObjectArray;
	public GameObject[] blankPuzzleArray;

	void Start () {
		UITable table = GetComponent<UITable> ();
		mChildList = table.children;

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

		CreatedPuzzleTableEvent (puzzleObjectArray);
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
