﻿using UnityEngine;
using System.Collections;

public abstract class Singleton <T> where T : class, new() {

	private static T m_Instance = null;

	public static T instance {
		get {
			if (m_Instance == null) {
				m_Instance = new T ();
			}
			return m_Instance;
		}
	}
}
