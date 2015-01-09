using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_UntilSleepTime : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int area_id;
		public string area_name;
		public int level_1;
		public int level_2;
		public int level_3;
		public int level_4;
		public int level_5;
		public int max;
	}
}