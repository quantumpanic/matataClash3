using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CampScript : MonoBehaviour {

	// Use this for initialization
	//public List<int> troops = new List<int>();
	int maxTroops = 5;
	int availableSlot;


	//Tar buang
	public Text capacityText;
	string a;

	void Start () {
		availableSlot = maxTroops;

		capacityText = gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
		
	}
	//Tar buang
	void Update(){
		a = "Army Camp ("+TroopsManager.Instance.troops.Count.ToString()+"/"+maxTroops+")";
		capacityText.text = a;

		if (!gameObject.GetComponent<BuildingScript>().isBuilding && !gameObject.GetComponent<BuildingScript>().isUpgrading &&  
				(inputManager.Instance.selectedEntity.avatar == gameObject) && !inputManager.Instance.selectedEntity.isBlueprint) {
			capacityText.gameObject.SetActive(true);
		} else {
			capacityText.gameObject.SetActive(false);
		}
	}
	
	public void addTroops (){
		//troops.Add(a);
		availableSlot--;
	}

	public bool isCampFull () {
		/*
		if(troops.Count < maxTroops) {
			print(troops.Count);
			return false;
		}
		else {
			print(troops.Count);
			return true;
		}
		*/
		if (availableSlot > 0) {
			return false;
		} else {
			return true;
		}
	}
	

	public int getAvailableSlot () {
		return availableSlot;
	}
}
