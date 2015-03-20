using UnityEngine;
using System.Collections;
using System;

public class GameMath  {

	public static double RoundOne(double count){
		count = Math.Round (count,1,MidpointRounding.AwayFromZero);
		return count;
	}

	public static double RoundZero(double count){
		count = Math.Round (count,0,MidpointRounding.AwayFromZero);
		return count;
	}
}
