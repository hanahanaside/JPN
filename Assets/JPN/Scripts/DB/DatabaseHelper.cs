using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class DatabaseHelper : MonoSingleton<DatabaseHelper> {

	public static event Action CreatedDatabaseEvent;

	public static readonly string DATABASE_FILE_NAME = "jpn.db";

	public const int DATABASE_VERSION = 1;

	public string filePath {
		get;
		set;
	}

	public string baseFilePath {
		get;
		set;
	}

	//パスの設定をする
	public override	void OnInitialize () {
		filePath = Application.persistentDataPath + "/" + DATABASE_FILE_NAME;
		baseFilePath = Application.streamingAssetsPath + "/" + DATABASE_FILE_NAME;

		#if UNITY_ANDROID && UNITY_EDITOR
		baseFilePath = "file://"+Path.Combine (Application.streamingAssetsPath, DATABASE_FILE_NAME);
		#endif

	}

	public void CreateDB () {
		//チュートリアルが終わってなかったら全てのデータを消す
		if (!PrefsManager.instance.TutorialFinished) {
			if (File.Exists (filePath)) {
				DeleteDB ();
			}
		}
		#if UNITY_IPHONE || UNITY_STANDALONE
		if (!File.Exists (filePath)) {
			CopyDB ();
			PrefsManager.instance.DatabaseVersion = DATABASE_VERSION;
			CreatedDatabaseEvent ();
			Debug.Log("copy db");
		} else {
			UpdateDatabase ();
		}
		#endif

		#if UNITY_ANDROID
		if (!File.Exists (filePath)) {
			StartCoroutine("CreateAndroidDatabase");
			PrefsManager.instance.DatabaseVersion = DATABASE_VERSION;
		}else {
			UpdateDatabase();
		}
		#endif
	}

	public void DeleteDB () {
		File.Delete (filePath);
	}

	public void CopyDB () {
		File.Copy (baseFilePath, filePath); 
	}

	private IEnumerator CreateAndroidDatabase () {
		WWW www = new WWW (baseFilePath);
		yield return www;
		File.WriteAllBytes (filePath, www.bytes);
		UpdateDatabase ();
	}

	private void UpdateDatabase () {
		Debug.Log ("update database ");
		int databaseVersion = PrefsManager.instance.DatabaseVersion;
		Debug.Log ("current version " + databaseVersion);
		switch (databaseVersion) {
		case 0:
			//秋葉原をインサート
			StageData stageData = new StageData ();
			stageData.AreaName = "秋葉原";
			stageData.Id = 48;
			stageData.IdolCount = 0;
			stageData.UpdatedDate = "";
			stageData.FlagConstruction = 1;
			stageData.AreaId = 8;
			StageDao dao = DaoFactory.CreateStageDao ();
			dao.InsertData (stageData);
			PrefsManager.instance.DatabaseVersion = 1;
			CreatedDatabaseEvent ();
			break;
		case 1:
			CreatedDatabaseEvent ();
			break;
		}

	}
}
