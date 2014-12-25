using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	public class Data{
		public int id;
		public string name;
	}

	void Start(){
		Data data = new Data ();
		data.id = 1;
		data.name = "name";
		string json = JsonFx.Json.JsonWriter.Serialize (data);
		data = JsonFx.Json.JsonReader.Deserialize<Data> (json);
		Debug.Log ("name " + data.name);
	}
}
