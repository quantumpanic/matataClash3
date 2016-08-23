using UnityEngine;
using System.Collections;

public class Cursor {

	private static Cursor instance = null; 
	protected Cursor() {}

	// Singleton pattern implementation
	public static Cursor Instance {
		get {
			if (instance == null) {
				instance = new Cursor();
			}  
			return instance;
		}
	}


	public GameObject selectedObject;
}
