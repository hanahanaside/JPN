﻿using UnityEngine;
using System.Collections;
using System;

public class GetIdleDialogManagerTutorial : MonoSingleton<GetIdleDialogManagerTutorial> {

	public static event Action ClosedEvent;

	private UILabel mTitleLabel;
	private UISprite mIdleSprite;
	private int mIdleId;
	private GameObject mDialogObject;

	void CompleteDismissEvent(){
		FenceManager.instance.HideFence ();
		mDialogObject.SetActive (false);
		gameObject.transform.localScale = new Vector3 (1,1,1);
		ClosedEvent ();
	}

	public override void OnInitialize () {
		mDialogObject = transform.FindChild ("Dialog").gameObject;
		mIdleSprite = mDialogObject.transform.FindChild ("IdleSprite").GetComponent<UISprite> ();
		mTitleLabel = mDialogObject.transform.FindChild ("TitleLabel").GetComponent<UILabel> ();
	}

	public void DebutButtonClicked () {
		iTweenEvent.GetEvent (gameObject,"DismissEvent").Play();
		CharacterVoiceManager.instance.PlayVoice (mIdleId - 1);
	}

	public void Show (int id) {
		mIdleId = id;
		StageDao dao = DaoFactory.CreateStageDao ();
		StageData	stage = dao.SelectById (mIdleId);
		stage.IdolCount++;
		if(string.IsNullOrEmpty(stage.UpdatedDate)){
			stage.UpdatedDate = DateTime.Now.ToString ();
		}
		dao.UpdateRecord (stage);
		mDialogObject.SetActive (true);
		mIdleSprite.spriteName = "idle_normal_" + id;
		System.Text.StringBuilder sb = new System.Text.StringBuilder ();
		sb.Append (stage.AreaName + "の子をスカウトした！");
		sb.Append ("\n");
		sb.Append (stage.IdolCount + " / 25");
		mTitleLabel.text = sb.ToString ();
		mIdleSprite.width = mIdleSprite.GetAtlasSprite ().width * 2;
		mIdleSprite.height = mIdleSprite.GetAtlasSprite ().height * 2;
		iTweenEvent.GetEvent (gameObject, "ShowEvent").Play ();
	}
}
