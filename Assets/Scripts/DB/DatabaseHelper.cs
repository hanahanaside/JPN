﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class DatabaseHelper : MonoSingleton<DatabaseHelper> {

	public static event Action CreatedDatabaseEvent;

	public static readonly string DATABASE_FILE_NAME = "jpn.db"; 

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

	public void CreateDB(){
		#if UNITY_IPHONE
		if (!File.Exists (filePath)) {
			CopyDB();
			CreatedDatabaseEvent ();
		}else {
			UpdateDatabase();
		}
		#endif

		#if UNITY_ANDROID
		if (!File.Exists (filePath)) {
			StartCoroutine("CreateAndroidDatabase");
		}
		#endif
	}

	public void DeleteDB(){
		File.Delete (filePath);
	}

	public void CopyDB(){
		File.Copy (baseFilePath, filePath); 
	}

	private IEnumerator CreateAndroidDatabase(){
		WWW www = new WWW (baseFilePath);
		yield return www;
		File.WriteAllBytes (filePath, www.bytes);
		CreatedDatabaseEvent ();
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
