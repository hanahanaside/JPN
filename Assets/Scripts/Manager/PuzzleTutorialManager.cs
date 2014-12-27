using UnityEngine;
using System.Collections;

public class PuzzleTutorialManager : MonoSingleton<PuzzleTutorialManager> {

	public Transform puzzleTableParent;

	public void StartTutorial () {
		GameObject puzzleTablePrefab = Resources.Load ("PuzzleTable/PuzzleTable_Tutorial") as GameObject;
		GameObject puzzleTableObject = Instantiate (puzzleTablePrefab)as GameObject;
		puzzleTableObject.transform.parent = puzzleTableParent.transform;
		puzzleTableObject.transform.localPosition = new Vector3 (0, 0, 0);
		puzzleTableObject.transform.localScale = new Vector3 (1, 1, 1);
		PuzzleTableTutorial puzzleTable = puzzleTableObject.GetComponent<PuzzleTableTutorial> ();
		puzzleTable.CreateTable ();
	}
}
