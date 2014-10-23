using UnityEngine;
using System.Collections;

public class MenuPanelManager : MonoSingleton<MenuPanelManager> {

	public GameObject dialogObject;

	public void ShowMenuPanel(){
		dialogObject.SetActive (true);
	}

	public void OnCloseButtonClicked(){
		dialogObject.SetActive (false);
	}
}
