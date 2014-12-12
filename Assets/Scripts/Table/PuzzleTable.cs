using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzleTable : MonoBehaviour {

	public static event Action<int[]> CreatedPuzzleTableEvent;

	private int[] mPuzzlePositionArray = { 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1 };
	private List<Transform> mChildList;
	private GameObject[] puzzleObjectArray;
	public GameObject[] blankPuzzleArray;
	public int[] puzzleIdArray;

	//パズルテーブルを作成する
	public void CreateTable (int level) {
		UITable table = GetComponent<UITable> ();
		mChildList = table.children;
		//生成するパズルの種類の数を決める
		int[] targetIdArray = null;
		//被らないようにする
		while (true) {
			targetIdArray = CreatePuzzleIdArray ();
			if (CheckNotDuplicate (targetIdArray)) {
				break;
			}
		}
			
		targetIdArray [0] = 7;
		targetIdArray [1] = 8;
		//パズルの配列を作成開始
		foreach (int targetId in targetIdArray) {
			GameObject puzzlePrefab = Resources.Load ("Puzzle/Puzzle_" + targetId) as GameObject;
			Puzzle puzzle = puzzlePrefab.GetComponent<Puzzle> ();

			//パズルを設置する最初のポジションインデックスを取得
			int startIndex = GetStartIndex (puzzle);

			//配列に数字を反映させる
			ApplyToArray (startIndex, puzzle);
		}
			
		//作成した配列通りにパズルを設置する
		PutPuzzle ();
			
		//残りの場所にブランクを設置
		AddEmptyPuzzle ();

		//テーブルを整列
		table.Reposition ();

		//コールバック
		CreatedPuzzleTableEvent (targetIdArray);
	}

	//配列通りにパズルを設置する
	private void PutPuzzle () {
		for (int i = 0; i < mPuzzlePositionArray.Length; i++) {
			int puzzleIndex = mPuzzlePositionArray [i];
			if (puzzleIndex <= 0) {
				continue;
			}
			Transform child = mChildList [i];
			GameObject puzzlePrefab = Resources.Load ("Puzzle/Puzzle_" + puzzleIndex) as GameObject;
			GameObject puzzleObject = Instantiate (puzzlePrefab) as GameObject;
			puzzleObject.transform.parent = child;
			puzzleObject.transform.localPosition = new Vector3 (0, 0, 0);
			puzzleObject.transform.localScale = new Vector3 (1, 1, 1);
			puzzleObject.SetActive (false);
		}
	}

	//パズルを設置可能な最初のインデックスを計算して返す
	private int GetStartIndex (Puzzle puzzle) {
		int startIndex = 0;
		int[] puzzleFormationArray = puzzle.puzzleFormationArray;
		bool complete = false;
		while (!complete) {
			startIndex = UnityEngine.Random.Range (0, 27);
			for (int i = 0; i < puzzleFormationArray.Length; i++) {
				//パズルの配列のサイズがポジションの要素数を超える場合はContinue
				if (startIndex + puzzleFormationArray.Length > mPuzzlePositionArray.Length) {
					break;
				}
				//設置しようとしたが、既に設置済みなので失敗
				if (puzzleFormationArray [i] != 0 && mPuzzlePositionArray [startIndex + i] != 0) {
					break;
				}
				//完成
				if (i == puzzle.puzzleFormationArray.Length - 1) {
					complete = true;
					break;
				}
			}

		}

		return startIndex;
	}

	//配列にパズルの数字を反映させる
	private void ApplyToArray (int startIndex, Puzzle puzzle) {
		foreach (int puzzleIndex in puzzle.puzzleFormationArray) {
			if (mPuzzlePositionArray [startIndex] == 0) {
				mPuzzlePositionArray [startIndex] = puzzleIndex;
			}
			startIndex++;
		}
	}
		
	//パズルのキャラが被っているかをチェックする
	private bool CheckNotDuplicate (int[] puzzleIdArray) {
		int puzzleId = puzzleIdArray [0];
		if (puzzleId == puzzleIdArray [1]) {
			return false;
		}
		return true;
	}
		
	//空のパズルを設置
	private void AddEmptyPuzzle () {
		int[] emptyPuzzleIndexArray = { 0, 0, 0, 0, 1 };
		for (int i = 0; i < mPuzzlePositionArray.Length; i++) {
			if (mPuzzlePositionArray [i] != 0) {
				continue;
			}
			int rand = UnityEngine.Random.Range (0, emptyPuzzleIndexArray.Length);
			int emptyPuzzleIndex = emptyPuzzleIndexArray [rand];
			Debug.Log ("ind " + emptyPuzzleIndex);
			Transform child = mChildList [i];
			GameObject	puzzleObject = Instantiate (blankPuzzleArray [emptyPuzzleIndex])as GameObject;
			puzzleObject.transform.parent = child;
			puzzleObject.transform.localPosition = new Vector3 (0, 0, 0);
			puzzleObject.transform.localScale = new Vector3 (1, 1, 1);
		}
	}
				
	//パズルIDを確率で計算して返す
	private int[] CreatePuzzleIdArray () {
		int[] targetIdArray = new int[2];
		int[] percentArray = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 5 };
		for (int i = 0; i < targetIdArray.Length; i++) {
			int rand = UnityEngine.Random.Range (0, percentArray.Length);
			int targetIndex = percentArray [rand];
			int targetId = puzzleIdArray [targetIndex];
			targetIdArray [i] = targetId;
		}
		return targetIdArray;
	}
}
