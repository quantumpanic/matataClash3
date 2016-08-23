using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

[Serializable]
public class BuildingLimit {
	public int townHallLevel;
	public int maxBarrack;
	public int maxGoldMine;
	public int maxCamp;
	public int maxWall; 	
}
public class BuildingManager : MonoBehaviour {

	public static BuildingManager Instance; 
	public BuildingLimit buildingLimit;
    void Awake()
    {
        if (!Instance) Instance = this;
    }

	public List<GameObject> buildingList = new List<GameObject>();

	public Button buildBarrackButton;
	public Button buildGoldMineButton;
	public Button buildCampButton;

	public void addBuilding (GameObject newBuilding){
		buildingList.Add(newBuilding);

		int max = 0;
		int cur = 0;
		Button b = null;
		switch(newBuilding.GetComponent<BuildingScript>().buildingType){
			case 1:	
				max = buildingLimit.maxBarrack;
				b = buildBarrackButton;
				break;
			case 2:	
				max = buildingLimit.maxCamp;
				b = buildCampButton;
				break;
			case 3:	
				max = buildingLimit.maxGoldMine;
				b = buildGoldMineButton;
				break;			
			case 4:	
				max = buildingLimit.maxWall;
				break;
		}
		foreach (GameObject go in buildingList) {		
			if(go.GetComponent<BuildingScript>().buildingType == newBuilding.GetComponent<BuildingScript>().buildingType)
				cur++;
		}

		if(cur >= max)
			if(b)
				b.gameObject.SetActive(false);


	}
/*
	public bool CheckIsBuildingMax (int buildingType){
		int max = 0;
		int cur = 0;
		switch(buildingType){
			case 0:	
				max = buildingLimit.maxBarrack;
				break;
			case 1:	
				max = buildingLimit.maxGoldMine;
				break;
			case 2:	
				max = buildingLimit.maxCamp;
				break;
			case 3:	
				max = buildingLimit.maxWall;
				break;
		}
		foreach (GameObject g in buildingList)
		{
			if(g.GetComponent<BuildingScript>().buildingType == buildingType)
				cur++;
		}
		if(cur<max)
			return true;
		else
			return false;
	}
*/	

}
