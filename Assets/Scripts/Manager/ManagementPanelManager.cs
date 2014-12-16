using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagementPanelManager : MonoSingleton<ManagementPanelManager>{

	public UIGrid grid;
	public UIScrollView scrollView;
	public GameObject dialogObject;
	public GameObject areaInfoCellPrefab;
	public UILabel totalCoinCountLabel;
	public UILabel generateCoinPowerLabel;
	public UILabel totalIdleCountLabel;

	public void ShowManagementPanel(){
		FenceManager.instance.ShowFence ();
		//ダイアログを表示
		dialogObject.SetActive (true);
		iTweenEvent.GetEvent (dialogObject,"ShowEvent").Play();
		//コインの総獲得数を設置
		totalCoinCountLabel.text = "" + GameMath.RoundZero (PlayerDataKeeper.instance.TotalCoinCount);
		//現在のコイン生成パワーをセット
		generateCoinPowerLabel.text = ""+GameMath.RoundOne (PlayerDataKeeper.instance.GenerateCoinPower);
		//全データを取得
		StageDao dao = DaoFactory.CreateStageDao ();
		List<Stage> stageList = dao.SelectAll ();
		//アイドルの総人数を設置
		int totalIdleCount = GetTotalIdleCount(stageList);
		totalIdleCountLabel.text =  totalIdleCount + "人";
		//エリアごとのアイドル情報のセルを設置
		foreach(Stage stage in stageList){
			GameObject areaInfoCell = Instantiate (areaInfoCellPrefab) as GameObject; 
			grid.AddChild (areaInfoCell.transform);
			areaInfoCell.transform.localScale = new Vector3 (1f,1f,1f);
			UILabel areaNameLabel = areaInfoCell.transform.Find ("AreaNameLabel").GetComponent<UILabel>();
			areaNameLabel.text = stage.AreaName;
			UILabel idolCountLabel = areaInfoCell.transform.Find ("IdleCountLabel").GetComponent<UILabel>();
			idolCountLabel.text = stage.IdleCount + "人";
		}
		scrollView.ResetPosition ();
	}
		
	public void OnTweetButtonClicked(){

	}

	public void OnCloseButtonClicked(){
		List<Transform> childList = grid.GetChildList ();
		for(int i = 4; i<childList.Count;i++){
			GameObject childObject = childList[i].gameObject;
			Destroy (childObject);
		}
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
	}

	private int GetTotalIdleCount(List<Stage> stageList){
		int totalIdleCount = 0;
		foreach(Stage stage in stageList){
			totalIdleCount += stage.IdleCount;
		}
		return totalIdleCount;
	}
}
