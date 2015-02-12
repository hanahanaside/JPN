using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ManagementPanelManager : MonoSingleton<ManagementPanelManager>{

	public UIGrid grid;
	public UIScrollView scrollView;
	public GameObject dialogObject;
	public GameObject areaInfoCellPrefab;
	public UILabel totalCoinCountLabel;
	public UILabel generateCoinPowerLabel;
	public UILabel totalIdleCountLabel;

	private int mTotalIdleCount;
	private double mTotalCoinCount;

	void CompleteDismissEvent(){
		List<Transform> childList = grid.GetChildList ();
		for(int i = 3; i<childList.Count;i++){
			GameObject childObject = childList[i].gameObject;
			Destroy (childObject);
		}
		FenceManager.instance.HideFence ();
		dialogObject.SetActive (false);
		dialogObject.transform.localScale = new Vector3 (1,1,1);
	}

	public void ShowManagementPanel(){
		FenceManager.instance.ShowFence ();
		//ダイアログを表示
		dialogObject.SetActive (true);
		iTweenEvent.GetEvent (dialogObject,"ShowEvent").Play();
		//コインの総獲得数を設置
		mTotalCoinCount = GameMath.RoundZero (PlayerDataKeeper.instance.TotalCoinCount);
		totalCoinCountLabel.text = "" + mTotalCoinCount;
		//現在のコイン生成パワーをセット
		generateCoinPowerLabel.text = ""+GameMath.RoundOne (PlayerDataKeeper.instance.GenerateCoinPower);
		//全データを取得
		StageDao dao = DaoFactory.CreateStageDao ();
		List<StageData> stageList = dao.SelectAll ();
		//アイドルの総人数を設置
		mTotalIdleCount = GetTotalIdleCount(stageList);
		totalIdleCountLabel.text =  mTotalIdleCount + "人";
		//エリアごとのアイドル情報のセルを設置
		foreach(StageData stage in stageList){
			GameObject areaInfoCell = Instantiate (areaInfoCellPrefab) as GameObject; 
			grid.AddChild (areaInfoCell.transform);
			areaInfoCell.transform.localScale = new Vector3 (1f,1f,1f);
			UILabel areaNameLabel = areaInfoCell.transform.Find ("AreaNameLabel").GetComponent<UILabel>();
			areaNameLabel.text = stage.AreaName;
			UILabel idolCountLabel = areaInfoCell.transform.Find ("IdleCountLabel").GetComponent<UILabel>();
			idolCountLabel.text = stage.IdolCount + "人";
		}
		scrollView.ResetPosition ();
	}
		
	public void OnTweetButtonClicked(){
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
		StringBuilder sb = new StringBuilder ();
		sb.Append ("私の経営するアイドルプロジェクトの在籍人数が" + mTotalIdleCount +"人、\n");
		sb.Append ("累計"+mTotalCoinCount +"円を稼ぎだしたよ！\n");
		sb.Append ("このゲーム超面白いからやってみて！→http://tt5.us/idolpro #あいぷろ");
		SocialConnector.Share(sb.ToString());
	}

	public void OnCloseButtonClicked(){
		iTweenEvent.GetEvent (dialogObject,"DismissEvent").Play();
		SoundManager.instance.PlaySE (SoundManager.SE_CHANNEL.Button);
	}

	private int GetTotalIdleCount(List<StageData> stageList){
		int totalIdleCount = 0;
		foreach(StageData stage in stageList){
			totalIdleCount += stage.IdolCount;
		}
		return totalIdleCount;
	}
}
