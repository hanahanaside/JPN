using UnityEngine;
using System.Collections;
using System;

public class GetIdleDialogManager : MonoBehaviour {

	public static event Action ClosedEvent;

	public UISprite mIdleSprite;
	private int mIdleId;

	public void OnCloseButtonClicked(){
		StageDao dao = DaoFactory.CreateStageDao ();
		Stage stage = dao.SelectById (mIdleId);
		stage.IdleCount++;
		stage.UpdatedDate = DateTime.Now.ToString ();
		dao.UpdateRecord (stage);
		FenceManager.instance.HideFence ();
		ClosedEvent ();
		Destroy (transform.parent.gameObject);
	}

	public void Show(int id){
		mIdleId = id;
		mIdleSprite.spriteName = "idle_normal_" + mIdleId;
		mIdleSprite.width = mIdleSprite.GetAtlasSprite ().width * 2;
		mIdleSprite.height = mIdleSprite.GetAtlasSprite ().height * 2;
	}
}
