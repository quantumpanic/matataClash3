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
	public Button buildBarrackButton;
	public Button buildGoldMineButton;
	public Button buildCampButton;
	public Button buildWallButton;
	public Button buildManaGeneratorButton;
}
public class BuildingManager : MonoBehaviour {
	public static BuildingManager Instance; 
	public BuildingLimit buildingLimit;
	public BuildButton buildButton;
	public List<GameObject> buildingList = new List<GameObject>();
	
    void Awake()
    {
        if (!Instance) Instance = this;
	}

	void Start(){
		//if(SceneManager.Instance.isCombatMap){
			Transform buildPanel = MapManager.Instance.gameObject.transform.GetChild(4).GetChild(1).GetChild(0);
			buildButton.buildBarrackButton = buildPanel.GetChild(2).GetComponent<Button>();
			buildButton.buildGoldMineButton = buildPanel.GetChild(0).GetComponent<Button>();
			buildButton.buildCampButton = buildPanel.GetChild(3).GetComponent<Button>();
			buildButton.buildWallButton = buildPanel.GetChild(4).GetComponent<Button>();
			buildButton.buildManaGeneratorButton = buildPanel.GetChild(1).GetComponent<Button>();
		//}

	}
	
	public void addBuilding (GameObject newBuilding){
		buildingList.Add(newBuilding);

		int max = 0;
		int cur = 0;
		Button b = null;
		switch(newBuilding.GetComponent<BuildingScript>().buildingType){
			case 1:	
				max = buildingLimit.maxBarrack;
				b = buildButton.buildBarrackButton;
				break;
			case 2:	
				max = buildingLimit.maxCamp;
				b = buildButton.buildCampButton;
				break;
			case 3:	
				max = buildingLimit.maxGoldMine;
				b = buildButton.buildGoldMineButton;
				break;			
			case 4:	
				max = buildingLimit.maxWall;
				b = buildButton.buildWallButton;
				break;
			case 5:	
				max = buildingLimit.maxManaGenerator;
				b = buildButton.buildManaGeneratorButton;
				break;
		}
		foreach (GameObject go in buildingList) {		
			if(go.GetComponent<BuildingScript>().buildingType == newBuilding.GetComponent<BuildingScript>().buildingType)
				cur++;
		}

		print(cur);
		if(cur >= max)
			if(b)
				b.gameObject.SetActive(false);


	}	

}
