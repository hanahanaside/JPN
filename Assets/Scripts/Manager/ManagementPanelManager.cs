using UnityEngine;
using System.Collections;

public class ManagementPanelManager : MonoSingleton<ManagementPanelManager>{

	public GameObject managementPanel;

	public void ShowManagementPanel(){
		FenceManager.instance.ShowFence ();
		managementPanel.SetActive (true);
	}
		
	public void OnTweetButtonClicked(){

	}

	public void OnCloseButtonClicked(){
		FenceManager.instance.HideFence ();
		managementPanel.SetActive (false);
	}
}
