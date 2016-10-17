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
	public int maxManaGenerator;
}
[Serializable]
public class BuildButton {
	public GameObject buildBarrackButton;
	public GameObject buildGoldMineButton;
	public GameObject buildCampButton;
	public GameObject buildWallButton;
	public GameObject buildManaGeneratorButton;
}
public class BuildingManager : MonoBehaviour {
	public static BuildingManager Instance; 
	public BuildingLimit buildingLimit;
	public BuildButton buildButton;
	public List<GameObject> buildingList = new List<GameObject>();
//	public static int z=0;
    void Awake()
    {
        if (!Instance) Instance = this;
	}

	void Start(){
		Transform buildPanel = MapManager.Instance.gameObject.transform.GetChild(4).GetChild(1).GetChild(0);
		buildButton.buildBarrackButton = buildPanel.GetChild(2).gameObject;
		buildButton.buildGoldMineButton = buildPanel.GetChild(0).gameObject;
		buildButton.buildCampButton = buildPanel.GetChild(3).gameObject;
		buildButton.buildWallButton = buildPanel.GetChild(4).gameObject;
		buildButton.buildManaGeneratorButton = buildPanel.GetChild(1).gameObject;
	}
	
	void Update(){
		if(doCheckBattle){
			if(buildingList.Count == 0)
				SceneManager.Instance.GoToScene("BattleResult");
		}
	}

	public bool doCheckBattle;

	public void addBuilding (GameObject newBuilding){
		
		buildingList.Add(newBuilding);

		int max = 0;
		int cur = 0;
		GameObject b = null;
		switch(newBuilding.GetComponent<BuildingScript>().buildingType){
			case BuildingScript.BuildingType.Barrack:	
				max = buildingLimit.maxBarrack;
				b = buildButton.buildBarrackButton;
				break;
			case BuildingScript.BuildingType.Camp:	
				max = buildingLimit.maxCamp;
				b = buildButton.buildCampButton;
				break;
			case BuildingScript.BuildingType.GoldMine:	
				max = buildingLimit.maxGoldMine;
				b = buildButton.buildGoldMineButton;
				break;			
			case BuildingScript.BuildingType.Wall:	
				max = buildingLimit.maxWall;
				b = buildButton.buildWallButton;
				break;
			case BuildingScript.BuildingType.ManaGenerator:	
				max = buildingLimit.maxManaGenerator;
				b = buildButton.buildManaGeneratorButton;
				break;
		}
		foreach (GameObject go in buildingList) {		
			if(go.GetComponent<BuildingScript>().buildingType == newBuilding.GetComponent<BuildingScript>().buildingType)
				cur++;
		}

		//print(cur);
		if(cur >= max)
			if(b)
				b.SetActive(false);


	}	

	public void RemoveWallFromList(){
		for(int n = (buildingList.Count-1); n >= 0; n--){
			if(buildingList[n].GetComponent<BuildingScript>().buildingType == BuildingScript.BuildingType.Wall)
				buildingList.RemoveAt(n);	
		}
		/*
		foreach(GameObject b in buildingList){
			if(b.GetComponent<BuildingScript>().buildingType == 4)
				buildingList.Remove(b);
		}
		*/
	}

}
