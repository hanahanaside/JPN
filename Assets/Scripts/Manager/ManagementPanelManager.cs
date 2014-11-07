using UnityEngine;
using System.Collections;

public class ManagementPanelManager : MonoSingleton<ManagementPanelManager>{

	public UIGrid grid;
	public GameObject dialogObject;
	public GameObject areaInfoCellPrefab;

	public void ShowManagementPanel(){
		FenceManager.instance.ShowFence ();
		dialogObject.SetActive (true);
		GameObject areaInfoCell = Instantiate (areaInfoCellPrefab) as GameObject; 
		grid.AddChild (areaInfoCell.transform);
		areaInfoCell.transform.localScale = new Vector3 (1f,1f,1f);
	}
		
	public void OnTweetButtonClicked(){

	}

	public void OnCloseButtonClicked(){
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
	}
}
