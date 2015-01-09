using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_GenerateCoinPower : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int area_id;
		public string area_name;
		public double level_1;
		public double level_2;
		public double level_3;
		public double level_4;
		public double level_5;
		public double max;
	}
}