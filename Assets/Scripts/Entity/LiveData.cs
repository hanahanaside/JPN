using UnityEngine;
using System.Collections;
using System;
public class LiveData
{

	public string startDate{ get; set; }

	public float time{ get; set; }

	public LiveData(){
		startDate = DateTime.Now.ToString ();
		time = 0;
	}
}
