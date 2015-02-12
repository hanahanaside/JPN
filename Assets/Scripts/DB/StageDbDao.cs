using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class StageDbDao : StageDao {

	private const string TABLE_NAME = "stage";
	private const string FIELD_ID = "id";
	private const string FIELD_AREA_NAME = "area_name";
	private const string FIELD_IDLE_COUNT = "idle_count";
	private const string FIELD_FLAG_CONSTRUCTION = "flag_construction";
	private const string FIELD_UPDATED_DATE = "updated_date";

	//存在する全てのステージデータを取得
	public List<StageData> SelectAll () {
		List<StageData> stageDataList = new List<StageData> ();
		SQLiteDB sqliteDB = OpenDatabase ();
		StringBuilder sb = new StringBuilder ();
		sb.Append ("select * from " + TABLE_NAME + " ");
		sb.Append ("where " + FIELD_IDLE_COUNT +  " != 0;");
		SQLiteQuery sqliteQuery = new SQLiteQuery (sqliteDB, sb.ToString ());
		while (sqliteQuery.Step ()) {
			StageData stage = GetStage (sqliteQuery);
			stageDataList.Add (stage);
		}
		sqliteQuery.Release ();
		sqliteDB.Close ();
		return stageDataList;
	}

	//IDでステージデータを取得
	public StageData SelectById (int id) {
		SQLiteDB sqliteDB = OpenDatabase ();
		StageData stage = new StageData ();
		StringBuilder sb = new StringBuilder ();
		sb.Append ("select * from " + TABLE_NAME + " ");
		sb.Append ("where " + FIELD_ID + " = " + id + ";");
		SQLiteQuery sqliteQuery = new SQLiteQuery (sqliteDB, sb.ToString ());
		while (sqliteQuery.Step ()) {
			stage = GetStage (sqliteQuery);
		}
		sqliteQuery.Release ();
		sqliteDB.Close ();
		return stage;
	}

	//レコード1行アップデート
	public void UpdateRecord (StageData stage) {
		SQLiteDB sqliteDB = OpenDatabase ();
		StringBuilder sb = new StringBuilder ();
		sb.Append ("update " + TABLE_NAME + " set ");
		sb.Append (FIELD_IDLE_COUNT + " = " + stage.IdolCount + ", ");
		sb.Append (FIELD_FLAG_CONSTRUCTION + " = " + stage.FlagConstruction + ", ");
		sb.Append (FIELD_UPDATED_DATE + " = '" + stage.UpdatedDate + "' ");
		sb.Append ("where " + FIELD_ID + " = " + stage.Id + ";");
		MyLog.LogDebug ("sql " + sb.ToString());
		SQLiteQuery sqliteQuery = new SQLiteQuery (sqliteDB, sb.ToString ());
		sqliteQuery.Step ();
		sqliteQuery.Release ();
		sqliteDB.Close ();
	}

	//全てのレコードのアップデートデートを統一する
	public void UpdateAllUpdateDate(string updateDate){
		SQLiteDB sqliteDB = OpenDatabase ();
		StringBuilder sb = new StringBuilder ();
		sb.Append ("update " + TABLE_NAME + " set ");
		sb.Append (FIELD_UPDATED_DATE + " = '" + updateDate + "' ");
		MyLog.LogDebug ("sql " + sb.ToString());
		SQLiteQuery sqliteQuery = new SQLiteQuery (sqliteDB, sb.ToString ());
		sqliteQuery.Step ();
		sqliteQuery.Release ();
		sqliteDB.Close ();
	}

	//クエリからステージデータを生成して返す
	private StageData GetStage (SQLiteQuery sqliteQuery) {
		StageData stage = new StageData ();
		// create date がnullの場合アリ
		try {
			stage.Id = sqliteQuery.GetInteger (FIELD_ID);
			stage.AreaName = sqliteQuery.GetString (FIELD_AREA_NAME);
			stage.IdolCount = sqliteQuery.GetInteger (FIELD_IDLE_COUNT);
			stage.FlagConstruction = sqliteQuery.GetInteger (FIELD_FLAG_CONSTRUCTION);
			stage.UpdatedDate = sqliteQuery.GetString (FIELD_UPDATED_DATE);
		} catch (Exception e) {
			MyLog.LogDebug (e.Message);
		} 
		return stage;
	}

	//データベースを開く
	private SQLiteDB OpenDatabase () {
		SQLiteDB sqliteDB = new SQLiteDB ();
		string fileName = Application.persistentDataPath + "/" + DatabaseHelper.DATABASE_FILE_NAME;
		sqliteDB.Open (fileName);
		return sqliteDB;
	}
}
