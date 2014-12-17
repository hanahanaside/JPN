using UnityEngine;
using System.Collections;
using System;

public class GetIdleDialogManager : MonoBehaviour {

	public static event Action ClosedEvent;

	public UISprite mIdleSprite;
	public UILabel titleLabel;
	private Stage mStage;

	public void OnCloseButtonClicked(){
		mStage.UpdatedDate = DateTime.Now.ToString ();
		StageDao dao = DaoFactory.CreateStageDao ();
		dao.UpdateRecord (mStage);
		FenceManager.instance.HideFence ();
		ClosedEvent ();
		transform.parent.gameObject.SetActive (false);
	}

	public void Show(int id){
		StageDao dao = DaoFactory.CreateStageDao ();
		mStage = dao.SelectById (id);
		mStage.IdleCount++;
		mIdleSprite.spriteName = "idle_normal_" + id;
		System.Text.StringBuilder sb = new System.Text.StringBuilder ();
		sb.Append (mStage.AreaName + "の子をスカウトした！");
		sb.Append ("\n");
		sb.Append ( mStage.IdleCount + " / 25");
		titleLabel.text = sb.ToString ();
		mIdleSprite.width = mIdleSprite.GetAtlasSprite ().width * 2;
		mIdleSprite.height = mIdleSprite.GetAtlasSprite ().height * 2;
	}
}
