using UnityEngine;
using System.Collections;

public class MyLog  {

	private static bool mIsDebug = true;

	public static void LogDebug(string message){
		if(!mIsDebug){
			return;
		}
		Debug.Log ("[my log] \n" +  message);
	}
}
