using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class DatabaseHelper : MonoSingleton<DatabaseHelper> {

	public static event Action CreatedDatabaseEvent;

	public const string DATABASE_FILE_NAME = "jpn.db"; 

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
		baseFilePath = "file://"+Path.Combine (Application.streamingAssetsPath, databaseFileName);
		#endif

		#if UNITY_EDITOR
		File.Delete(filePath);
		#endif
	}

	public void CreateDatabase(){
		#if UNITY_IPHONE
		if (!File.Exists (filePath)) {
			File.Copy (baseFilePath, filePath); 
			CreatedDatabaseEvent ();
		}else {
			UpdateDatabase();
		}
		#endif
	}

	private void UpdateDatabase(){
		int databaseVersion = PrefsManager.instance.DatabaseVersion;
		switch(databaseVersion){
		case 0:
			CreatedDatabaseEvent ();
			break;
		}
	}
}
