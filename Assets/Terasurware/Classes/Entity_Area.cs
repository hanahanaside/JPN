using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Area : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int area_id;
		public string area_name;
		public int area_open;
		public int minimum_amount;
		public int cost_start;
		public int cost_add;
		public int cost_end;
	}
}