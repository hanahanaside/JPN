using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDbDao : StageDao {

	private const string TABLE_NAME = "stage";
	private const string FIELD_ID = "id";
	private const string FIELD_AREA_NAME = "area_name";
	private const string FIELD_IDLE_COUNT = "idle_count";
	private const string FIELD_FLAG_CONSTRUCTION = "flag_construction";
	private const string FIELD_CREATED_DATE = "created_date";

	public List<Stage> SelectAll(){
		List<Stage> stageDataList = new List<Stage> ();
		for (int i = 1; i <= 47; i++) {
			Stage stageData = new Stage ();
			stageData.Id = i;
			stageData.IdleCount = 10;
			stageData.AreaName = AreaName (i);
			stageData.FlagConstruction = 1;
			stageDataList.Add (stageData);
		}
		return stageDataList;
	}

	public void UpdateById(Stage stage){

	}

	//ダミーデータ
	private string AreaName (int id) {
		string areaName = "";
		switch (id) {
		case 1:
			areaName = "北海道";
			break;
		case 2:
			areaName = "青森";
			break;
		case 3:
			areaName = "岩手";
			break;
		case 4:
			areaName = "宮城";
			break;
		case 5:
			areaName = "秋田";
			break;
		case 6:
			areaName = "山形";
			break;
		case 7:
			areaName = "福島";
			break;
		case 8:
			areaName = "茨城";
			break;
		case 9:
			areaName = "栃木";
			break;
		case 10:
			areaName = "群馬";
			break;
		case 11:
			areaName = "埼玉";
			break;
		case 12:
			areaName = "千葉";
			break;
		case 13:
			areaName = "東京";
			break;
		case 14:
			areaName = "神奈川";
			break;
		case 15:
			areaName = "新潟";
			break;
		case 16:
			areaName = "富山";
			break;
		case 17:
			areaName = "石川";
			break;
		case 18:
			areaName = "福井";
			break;
		case 19:
			areaName = "山梨";
			break;
		case 20:
			areaName = "長野";
			break;
		case 21:
			areaName = "岐阜";
			break;
		case 22:
			areaName = "静岡";
			break;
		case 23:
			areaName = "愛知";
			break;
		case 24:
			areaName = "三重";
			break;
		case 25:
			areaName = "滋賀";
			break;
		case 26:
			areaName = "京都";
			break;
		case 27:
			areaName = "大阪";
			break;
		case 28:
			areaName = "兵庫";
			break;
		case 29:
			areaName = "奈良";
			break;
		case 30:
			areaName = "和歌山";
			break;
		case 31:
			areaName = "鳥取";
			break;
		case 32:
			areaName = "島根";
			break;
		case 33:
			areaName = "岡山";
			break;
		case 34:
			areaName = "広島";
			break;
		case 35:
			areaName = "山口";
			break;
		case 36:
			areaName = "徳島";
			break;
		case 37:
			areaName = "香川";
			break;
		case 38:
			areaName = "愛媛";
			break;
		case 39:
			areaName = "高知";
			break;
		case 40:
			areaName = "福岡";
			break;
		case 41:
			areaName = "佐賀";
			break;
		case 42:
			areaName = "長崎";
			break;
		case 43:
			areaName = "熊本";
			break;
		case 44:
			areaName = "大分";
			break;
		case 45:
			areaName = "宮崎";
			break;
		case 46:
			areaName = "鹿児島";
			break;
		case 47:
			areaName = "沖縄";
			break;
		}
		return areaName;
	}
}
