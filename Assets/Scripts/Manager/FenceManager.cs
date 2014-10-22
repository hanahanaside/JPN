using UnityEngine;
using System.Collections;

public class FenceManager : MonoSingleton<FenceManager> {

	public GameObject fencePanel;

	public void ShowFence(){
		fencePanel.SetActive (true);
	}

	public void HideFence(){
		fencePanel.SetActive (false);
	}
}
