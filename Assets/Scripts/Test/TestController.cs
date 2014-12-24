using UnityEngine;
using System.Collections;


public class TestController : MonoBehaviour {

	void Start(){
		Entity_News news = Resources.Load ("Data/News") as Entity_News;
		Debug.Log ("" + news.param[0].message);
	}
}
