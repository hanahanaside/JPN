using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class StageDbDao : StageDao {

	private const string TABLE_NAME = "stage";
	private const string ID = "id";
	private const string AREA_NAME = "area_name";
	private const string IDLE_COUNT = "idle_count";
	private const string STATE = "state";
	private const string UPDATED_DATE = "updated_date";

	//存在する全てのステージデータを取得
	public List<StageData> SelectAll () {
		List<StageData> stageDataList = new List<StageData> ();
		SQLiteDB sqliteDB = OpenDatabase ();
		StringBuilder sb = new StringBuilder ();
		sb.Append ("select * from " + TABLE_NAME + " ");
		sb.Append ("where " + IDLE_COUNT + " != 0;");
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
		sb.Append ("where " + ID + " = " + id + ";");
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
		sb.Append (IDLE_COUNT + " = " + stage.IdleCount + ", ");
		sb.Append (STATE + " = " + ((int)stage.State) + ", ");
		sb.Append (UPDATED_DATE + " = '" + stage.UpdatedDate + "' ");
		sb.Append ("where " + ID + " = " + stage.Id + ";");
		MyLog.LogDebug ("sql " + sb.ToString ());
		SQLiteQuery sqliteQuery = new SQLiteQuery (sqliteDB, sb.ToString ());
		sqliteQuery.Step ();
		sqliteQuery.Release ();
		sqliteDB.Close ();
	}
		
	//全てのレコードのアップデートデートを統一する
	public void UpdateAllUpdateDate (string updateDate) {
		SQLiteDB sqliteDB = OpenDatabase ();
		StringBuilder sb = new StringBuilder ();
		sb.Append ("update " + TABLE_NAME + " set ");
		sb.Append (UPDATED_DATE + " = '" + updateDate + "' ");
		MyLog.LogDebug ("sql " + sb.ToString ());
		SQLiteQuery sqliteQuery = new SQLiteQuery (sqliteDB, sb.ToString ());
		sqliteQuery.Step ();
		sqliteQuery.Release ();
		sqliteDB.Close ();
	}

	//指定したカラムの全データを取得
	public List<int> SelectByColumn (string columnName) {
		List<int> dataList = new List<int> ();
		SQLiteDB sqliteDB = OpenDatabase ();

		string sql = "select " + columnName + " from " + TABLE_NAME + ";";
		SQLiteQuery sqliteQuery = null;
		//カラムが存在していない場合はNullを返す
		try {
			sqliteQuery = new SQLiteQuery (sqliteDB, sql);
		} catch (Exception e) {
			Debug.Log ("" + e.Message);
			return null;
		}
		while (sqliteQuery.Step ()) {
			dataList.Add (sqliteQuery.GetInteger (columnName));
		}
		return dataList;
	}

	//クエリからステージデータを生成して返す
	private StageData GetStage (SQLiteQuery sqliteQuery) {
		StageData stage = new StageData ();
		// create date がnullの場合アリ
		try {
			stage.Id = sqliteQuery.GetInteger (ID);
			stage.AreaName = sqliteQuery.GetString (AREA_NAME);
			stage.IdleCount = sqliteQuery.GetInteger (IDLE_COUNT);
			stage.State = (StageData.StateType)sqliteQuery.GetInteger (STATE);
			stage.UpdatedDate = sqliteQuery.GetString (UPDATED_DATE);
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
