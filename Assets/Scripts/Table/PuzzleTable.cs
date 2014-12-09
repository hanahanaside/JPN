using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzleTable : MonoBehaviour {

	public static event Action<GameObject[]> CreatedPuzzleTableEvent;

	private List<Transform> mChildList;
	private GameObject[] puzzleObjectArray;
	public GameObject[] blankPuzzleArray;

	//パズルテーブルを作成する
	public void CreateTable (int level) {
		UITable table = GetComponent<UITable> ();
		mChildList = table.children;
		//生成するパズルの種類の数を決める
		int[] puzzleIdArray = null;
		//被らないようにする
		while (true) {
			puzzleIdArray = CreatePuzzleIdArray ();
			if(CheckNotDuplicate(puzzleIdArray)){
				break;
			}
		}
		puzzleObjectArray = new GameObject[puzzleIdArray.Length];
		for (int i = 0; i < puzzleObjectArray.Length; i++) {
			int puzzleId = puzzleIdArray [i];
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

		CreatedPuzzleTableEvent (puzzleObjectArray);
	}

	//パズルのキャラが被っているかをチェックする
	private bool CheckNotDuplicate (int[] puzzleIdArray) {
		int puzzleId = puzzleIdArray[0];
		if(puzzleId == puzzleIdArray[1]){
			return false;
		}
		return true;
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

	//キャラが被らないようにパズルIDをを返す
	private int[] CreatePuzzleIdArray () {
		int[] puzzleIdArray = new int[2];
		for (int i = 0; i < puzzleIdArray.Length; i++) {
			int rand = UnityEngine.Random.Range (1, 11);
			int puzzleId = 0;
			switch (rand) {
			case 1:
			case 2:
				puzzleId = 1;
				break;
			case 3:
			case 4:
				puzzleId = 2;
				break;
			case 5:
			case 6:
				puzzleId = 3;
				break;
			case 7:
			case 8:
				puzzleId = 4;
				break;
			case 9:
				puzzleId = 5;
				break;
			case 10:
				puzzleId = 6;
				break;
			}
			puzzleIdArray [i] = puzzleId;
		}
		return puzzleIdArray;
	}
}
