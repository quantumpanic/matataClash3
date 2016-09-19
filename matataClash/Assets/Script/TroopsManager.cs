using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TroopsManager : ScriptableObject {
private static TroopsManager instance = null; 
	protected TroopsManager() {}

	// Singleton pattern implementation
	public static TroopsManager Instance {
		get {
			if (instance == null) {
//				instance = new GameManagerScript();
				instance = ScriptableObject.CreateInstance <TroopsManager> ();
			}  
			return instance;
		}
	}

	List<GameObject> campList = new List<GameObject>();
	int availableCampSlot = 0;
	public List<int> troops = new List<int>();
	public void addCamp (GameObject newCamp) {
		campList.Add(newCamp);
		availableCampSlot += newCamp.GetComponent<CampScript>().getAvailableSlot();
		Debug.Log(availableCampSlot);
		troops.AddRange(newCamp.GetComponent<CampScript>().campedTroops);
	}

	public void addTroops (int a) {
		if  (availableCampSlot > 0) {
			troops.Add(a);
			availableCampSlot --;
			foreach (GameObject camp in campList) {
				if (!camp.GetComponent<CampScript>().isCampFull()){
					//troops masukan ke camp
					camp.GetComponent<CampScript>().addTroops(a);

					Debug.Log("ss");
					break;
				}
			}
		}
		
	}

	/*
	campslotindex=0
	foreach go g in campList
		
		int camplv = g.campscript.lv
		campslotindex += 5xcamplv
		
	*/

	public bool isAllCampFull () {
		if (availableCampSlot > 0) {
			return false;
		}
		else {
			return true;
		}
	}

}