using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : ScriptableObject {

	private static BuildingManager instance = null; 
	protected BuildingManager() {}

	// Singleton pattern implementation
	public static BuildingManager Instance {
		get {
			if (instance == null) {
//				instance = new GameManagerScript();
				instance = ScriptableObject.CreateInstance <BuildingManager> ();
			}  
			return instance;
		}
	}

	List<GameObject> buildingList = new List<GameObject>();

	public void addBuilding (GameObject newBuilding){
		buildingList.Add(newBuilding);

		foreach (GameObject g in buildingList)
		{
			Debug.Log(g.name+" "+g.GetComponent<BuildingScript>().buildingType.ToString());
		}
	}

}
