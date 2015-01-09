using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ConstructionTime : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int area_id;
		public string area_name;
		public int time;
	}
}