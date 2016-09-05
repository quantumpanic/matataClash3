using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class DataReader : MonoBehaviour {
	public static DataReader Instance; 
    void Awake() {
        if (!Instance) Instance = this;
    }
	
	//public List<TestJSON> testList = new List<TestJSON>();
	public List<ArmyBuilding> armyBuildingList = new List<ArmyBuilding>();
	public List<ResourceBuilding> resourceBuildingList = new List<ResourceBuilding>();
	
	// Use this for initialization
	void Start(){
		foreach(var asset in Resources.LoadAll<TextAsset>("data/resource buildings")){
			ResourceBuilding t = ResourceBuilding.CreateFromJSON(asset.text);
			resourceBuildingList.Add(t);
		}

		foreach(var asset in Resources.LoadAll<TextAsset>("data/army buildings")){
			ArmyBuilding t = ArmyBuilding.CreateFromJSON(asset.text);
			armyBuildingList.Add(t);
		}

		foreach (ArmyBuilding t in armyBuildingList){
			print(t.troopCapacity);
		}

		foreach (ResourceBuilding t in resourceBuildingList){
			print(t.buildTime);
		}
	}

}

[System.Serializable]
public class Army {
	public int unitType;
	public string name;
	public int level;
	public int damagePerSecond;
	public int damagePerAttack;
	public int hitpoint;
	public int trainingCost;
	public int researchCost;
	public int laboratoryLevelRequired;
	public long researchTime;
	public int preferredTarget;
	public int attackType;
	public int housingSpace;
	public long trainingTime;
	public int movementSpeed;
	public float attackSpeed;
	public int barrackLevelRequired;
	public float range;
	public string description;

	public void CreateFromJSON(string jsonString){
		JsonUtility.FromJson<Army>(jsonString);
	}

}

[System.Serializable]
public class BuildingBase {
	public int unitType;
	public string name;
	public int level;
	public int hitpoint;
	public int buildCost;
	public int buildTime;
	public int experienceGained;
	public int townHallLevelRequired;
	public int townHallLevel;
	public int numberAvailable;
	public int size;
	public string description;
}

[System.Serializable]
public class ArmyBuilding : BuildingBase {
	public int troopCapacity;

	public static ArmyBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<ArmyBuilding>(jsonString);
	}
}

[System.Serializable]
public class ResourceBuilding : BuildingBase {
	public int boostCost;
	public int capacity;
	public int productionRate;
	public long timeToFill;
	public int catchUpPoint;
	
	public static ResourceBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<ResourceBuilding>(jsonString);
	}
}

[System.Serializable]
public abstract class T {
	public int Level; 
}

[System.Serializable]
public class TestJSON : T {

	public static TestJSON C(string a){
		return JsonUtility.FromJson<TestJSON>(a);
	}
}


