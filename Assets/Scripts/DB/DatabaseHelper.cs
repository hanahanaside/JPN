using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class DatabaseHelper : MonoSingleton<DatabaseHelper> {


	public static event Action CreatedDatabaseEvent;

	public const int DATABASE_VERSION = 0;
	public static readonly string DATABASE_FILE_NAME = "jpn.db";

	private delegate void CompleteCopyDelegate ();

	private CompleteCopyDelegate mCompleteCopyDelegate;

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
				File.Delete (filePath);
			}
		}

		//無ければ作る
		if (!File.Exists (filePath)) {
			mCompleteCopyDelegate = () => {
				PrefsManager.instance.DatabaseVersion = DATABASE_VERSION;
				CreatedDatabaseEvent ();
			};
			StartCoroutine ("CopyDatabase");
		} 
		//既に存在していればアップデートの確認
		else {
			UpdateDatabase ();
		}
	}

	//DBを再構築する
	public void RecreateDatabase () {
		File.Delete (filePath);
		StartCoroutine ("CopyDatabase");
	}

	private IEnumerator CopyDatabase () {
		#if UNITY_IPHONE
		yield return null;
		File.Copy (baseFilePath, filePath); 
		#endif
		#if UNITY_ANDROID
		WWW www = new WWW (baseFilePath);
		yield return www;
		File.WriteAllBytes (filePath, www.bytes);
		#endif
		if (mCompleteCopyDelegate != null) {
			mCompleteCopyDelegate ();
		}
	}

	private void UpdateDatabase () {
		int databaseVersion = PrefsManager.instance.DatabaseVersion;
		Debug.Log ("現在のデータベースバージョンは" + databaseVersion);
		switch (databaseVersion) {
		case 0:
			RenameFlagConstructionToState ();
			CreatedDatabaseEvent ();
			break;
		}
	}

	//FlagConstructionカラムをStateにリネームする
	private void RenameFlagConstructionToState () {
		Debug.Log ("Flag_ConstructionをStateに変更開始");
		StageDao dao = DaoFactory.CreateStageDao ();
		//データを一時的に避難させる
		List<int> flagConstructionList = dao.SelectByColumn ("flag_construction");
		//既にFlagConstructionカラムがなければ何もしない(データベースバージョンを保存し忘れてしまいました。)
		if (flagConstructionList == null) {
			return;
		}
		//データを一時的に避難させる
		List<StageData> stageDataList = dao.SelectAll ();
		//コピー完了コールバック
		mCompleteCopyDelegate = () => {
			for (int i = 0; i < stageDataList.Count; i++) {
				StageData stageData = stageDataList [i];
				stageData.UpdatedDate = DateTime.Now.ToString();
				int flagConstruction = flagConstructionList [i];
				//工事が完了している場合はステートをノーマルにする
				if (flagConstruction == 0) {
					stageData.State = StageData.StateType.Normal;
				}
				dao.UpdateRecord (stageData);
			}
			PrefsManager.instance.DatabaseVersion = 0;
			CreatedDatabaseEvent ();
			Debug.Log ("Flag_ConstructionをStateに変更完了");
		};
		File.Delete (filePath);
		StartCoroutine ("CopyDatabase");
	}
}
