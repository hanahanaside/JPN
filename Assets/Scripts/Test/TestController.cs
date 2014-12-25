using UnityEngine;
using System.Collections;
using System;

public class TestController : MonoBehaviour {

	public class Data{
		public	int id;
		public	string name;
	}

	void Start(){
		Data data = new Data ();
		data.id = 1;
		data.name = "aaaa";
		string json = MiniJSON.Json.Serialize (data);

		PlayerPrefs.SetString ("json",json);
		json = PlayerPrefs.GetString ("json");
	}
}
