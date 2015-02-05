using UnityEngine;
using System.Collections;

public class MyLog {

	private static bool mIsDebug = false;

	public static void LogDebug(string message){
		if(!mIsDebug){
			return;
		}
		Debug.Log ("[my log] \n" +  message);
	}
}
