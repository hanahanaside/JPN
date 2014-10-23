using UnityEngine;
using System.Collections;

public class ManagementPanelManager : MonoSingleton<ManagementPanelManager>{

	public GameObject dialogObject;

	public void ShowManagementPanel(){
		FenceManager.instance.ShowFence ();
		dialogObject.SetActive (true);
	}
		
	public void OnTweetButtonClicked(){

	}

	public void OnCloseButtonClicked(){
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
	}
}
