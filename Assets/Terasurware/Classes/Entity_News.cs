using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_News : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int news_id;
		public int reward;
		public string message;
		public string unit;
	}
}