using UnityEngine;
using System.Collections;
using System;

public static class TimeSpanCalculator {

	public static float CalcFromNow(string date){
		DateTime dtThen = DateTime.Parse (date);
		DateTime dtNow = DateTime.Now;
		TimeSpan ts = dtNow - dtThen;
		return (float)ts.TotalSeconds;
	}
}
