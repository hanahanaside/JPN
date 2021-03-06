﻿using UnityEngine;
using System.Collections;

public class OKDialog : MonoSingleton<OKDialog> {

	public delegate void okButtonClickedDelegate();
	public UILabel titleLabel;
	public GameObject dialogObject;
	private okButtonClickedDelegate mOKButtonClicked;
	private GameObject mFenceObject;

	void CompleteDismissEvent(){
		dialogObject.SetActive (false);
		dialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public override void OnInitialize(){
		mFenceObject = transform.FindChild ("Fence").gameObject;
	}

	public void Show(string title){
		dialogObject.SetActive (true);
		mFenceObject.SetActive (true);
		titleLabel.text = title;
		iTweenEvent.GetEvent (dialogObject,"ShowEvent").Play();
	}

	public void OKButtonClicked(){
		mFenceObject.SetActive (false);
		iTweenEvent.GetEvent (dialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		if(mOKButtonClicked != null){
			mOKButtonClicked();
		}
	}

	public okButtonClickedDelegate OnOKButtonClicked{
		set{
			mOKButtonClicked = value;
		}
	}
}
