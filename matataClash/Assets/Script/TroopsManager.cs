using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TroopsManager : MonoBehaviour {
	public static TroopsManager Instance; 
/*	protected TroopsManager() {}

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
*/
	public GameObject footmanPrefab;
	public GameObject querychanPrefab;
	GameObject newTroops;
	public List<GameObject> campList = new List<GameObject>();
	public int availableCampSlot = 0;
	public List<GameObject> troops = new List<GameObject>();
	public List<GameObject> survivingTroops = new List<GameObject>();

	void Awake(){
        if (!Instance) Instance = this;
    }

	public void UpdateTroops(){
		troops.Clear();
		foreach(GameObject camp in campList){
			troops.AddRange(camp.GetComponent<CampScript>().campedTroops);
		}
	}

	public void addCamp (GameObject newCamp) {
		campList.Add(newCamp);
		//availableCampSlot += newCamp.GetComponent<CampScript>().availableSlot;
		availableCampSlot += newCamp.GetComponent<CampScript>().getAvailableSlot();
		Debug.Log(availableCampSlot);
		//troops.AddRange(newCamp.GetComponent<CampScript>().campedTroops);
	}

	public void addTroops (int troopsID) {
		switch(troopsID){
			case 1:
				newTroops = footmanPrefab;
				break;
			case 2:
				newTroops = querychanPrefab;
				break;
		}

		if  (availableCampSlot > 0) {
			//troops.Add(newTroops);
			availableCampSlot --;
			int n = 0;
			foreach (GameObject camp in campList) {
				if (!camp.GetComponent<CampScript>().isCampFull()){
					//troops masukan ke camp
					GameObject t = Instantiate(newTroops,Vector3.one * 1000,Quaternion.identity) as GameObject;
					camp.GetComponent<CampScript>().addTroops(t);
					t.transform.GetChild(0).GetComponent<TroopScript>().campIndex = n;
					UpdateTroops();
					//Debug.Log("ss");
					break;
				}
				n++;
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