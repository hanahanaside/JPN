using UnityEngine;
using System.Collections;

public class MenuPanelManager : MonoSingleton<MenuPanelManager> {

	public GameObject menuPanel;

	public void ShowMenuPanel(){
		menuPanel.SetActive (true);
	}

	public void OnCloseButtonClicked(){
		menuPanel.SetActive (false);
	}
}
