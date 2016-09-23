using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CampScript : MonoBehaviour {
	public List<GameObject> campedTroops = new List<GameObject>();
	int maxTroops = 5;
	public int availableSlot;
	public Text capacityText;
	string a;

	void Awake()
	{
		availableSlot = maxTroops;
	}

	void Start () {

		capacityText = gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
		
	}

	void Update(){
		availableSlot = maxTroops - campedTroops.Count;
		a = "Army Camp ("+campedTroops.Count.ToString()+"/"+maxTroops+")";
		capacityText.text = a;

		if (!gameObject.GetComponent<BuildingScript>().isBuilding && !gameObject.GetComponent<BuildingScript>().isUpgrading &&  
				(inputManager.Instance.selectedEntity.avatar == gameObject) && !inputManager.Instance.selectedEntity.isBlueprint
				&& !SceneManager.Instance.isCombatMap) {
			capacityText.gameObject.SetActive(true);
		} else {
			capacityText.gameObject.SetActive(false);
		}
	}
	
	public void addTroops (GameObject newTroops){
		campedTroops.Add(newTroops);
		//availableSlot--;
	}

	public bool isCampFull () {
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
