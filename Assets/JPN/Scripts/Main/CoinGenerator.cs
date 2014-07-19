using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoBehaviour {

	public GameObject coinPanel;
	public GameObject coinPrefab;

	IEnumerator Start () {
		yield return new WaitForSeconds (10f);
		GameObject coinObject = Instantiate (coinPrefab) as GameObject;
		coinObject.transform.parent = coinPanel.transform;
		coinObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		StartCoroutine(Start());
		UICenterOnChild.OnCenterCallback a  = A;
		a(gameObject);
	}

	private void A(GameObject ga){
		Debug.Log("asss");
	}
}
