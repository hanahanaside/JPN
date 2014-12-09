using UnityEngine;
using System.Collections;
using System;

public class GetIdleDialogManager : MonoBehaviour {

	public static event Action ClosedEvent;
	private int mIdleId;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
	}
}
