using UnityEngine;
using System.Collections;
using System.Text;

public static class TimeConverter {

	public static string Convert (float timeSeconds) {
		int time = (int)timeSeconds;
		int h = time / (60 * 60); //1時間は3600秒
		int eh = time % (60 * 60); //timeを3600で割ったあまり
		int m = eh / 60; //ehを60で割る．1分は，60秒
		int sec = eh % 60; //ehを60で割ったあまり
		StringBuilder sb = new StringBuilder ();
		if (h > 0) {
			sb.Append (h + "時間");
		}
		if (m > 0) {
			sb.Append (m + "分");
		}
		if (sec >= 0) {
			sb.Append (sec + "秒");
		}
		return sb.ToString ();
	}
}
