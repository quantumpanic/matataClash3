using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class DataReader : MonoBehaviour {
	public static DataReader Instance; 
    void Awake() {
        if (!Instance) Instance = this;
    }
	
	public List<ArmyProducerBuilding> armyProducerBuildingList = new List<ArmyProducerBuilding>();
	public List<ArmyStorageBuilding> armyStorageBuildingList = new List<ArmyStorageBuilding>();
	public List<DefenseBuilding> defenseBuildingList = new List<DefenseBuilding>();
	public List<TrapBuilding> trapBuildingList = new List<TrapBuilding>();
	public List<ResourceProducerBuilding> resourceProducerBuildingList = new List<ResourceProducerBuilding>();
	public List<ResourceStorageBuilding> resourceStorageBuildingList = new List<ResourceStorageBuilding>();
	public List<TownHallBuilding> townHallBuildingList = new List<TownHallBuilding>();
	public List<Army> armyList = new List<Army>();
	
	// Use this for initialization
	void Start(){
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/buildings/army buildings/produsen")){
			ArmyProducerBuilding b = ArmyProducerBuilding.CreateFromJSON(asset.text);
			armyProducerBuildingList.Add(b);
		}
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/buildings/army buildings/storage")){
			ArmyStorageBuilding b = ArmyStorageBuilding.CreateFromJSON(asset.text);
			armyStorageBuildingList.Add(b);
		}
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/buildings/defenses/buildings")){
			DefenseBuilding b = DefenseBuilding.CreateFromJSON(asset.text);
			defenseBuildingList.Add(b);
		}
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/buildings/defenses/traps")){
			TrapBuilding b = TrapBuilding.CreateFromJSON(asset.text);
			trapBuildingList.Add(b);
		}
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/buildings/resource/produsen")){
			ResourceProducerBuilding t = ResourceProducerBuilding.CreateFromJSON(asset.text);
			resourceProducerBuildingList.Add(t);
		}
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/buildings/resource/storage")){
			ResourceStorageBuilding t = ResourceStorageBuilding.CreateFromJSON(asset.text);
			resourceStorageBuildingList.Add(t);
		}
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/buildings/resource/pusat")){
			TownHallBuilding t = TownHallBuilding.CreateFromJSON(asset.text);
			townHallBuildingList.Add(t);
		}
		foreach(var asset in Resources.LoadAll<TextAsset>("unit kategory/army")){
			Army a = Army.CreateFromJSON(asset.text);
			armyList.Add(a);
		}

		Debug.Log(
			armyProducerBuildingList.Count+"  "+
			armyStorageBuildingList.Count+"  "+
			defenseBuildingList.Count+"  "+
			trapBuildingList.Count+"  "+
			resourceProducerBuildingList.Count+"  "+
			resourceStorageBuildingList.Count+"  "+
			armyList.Count
		);
	}

}

[System.Serializable]
public class Army {
	public int unitType;
	public string name;
	public int level;
	public float damagePerAttack;
	public float maxHitpoint;
	public int trainingCost;
	public int researchCost;
	public int laboratoryLevelRequired;
	public float researchTime;
	public int preferredTarget;
	public int attackType;
	public int housingSpace;
	public float trainingTime;
	public float movementSpeed;
	public float attackSpeed;
	public int barrackLevelRequired;
	public float range;
	public string description;

	public static Army CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<Army>(jsonString);
	}

}

[System.Serializable]
public class BuildingBase {
	public int unitType;
	public string name;
	public int level;
	public float maxHitpoint;
	public int buildCost;
	public float buildTime;
	public int experienceGained;
	public int townHallLevelRequired;
	public string description;
}

[System.Serializable]
public class ArmyProducerBuilding : BuildingBase {
	public int boostCost;
	public int maximumUnitQueueLength;
	public string[] maximumUnitQueueItems;

	public static ArmyProducerBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<ArmyProducerBuilding>(jsonString);
	}
}

[System.Serializable]
public class ArmyStorageBuilding : BuildingBase {
	public int troopsCapacity;

	public static ArmyStorageBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<ArmyStorageBuilding>(jsonString);
	}
}

[System.Serializable]
public class DefenseBuilding : BuildingBase {
	public float damage;
	public float range;
	public int damageType;
	public int unitTypeTarget;
	public float attackSpeed;

	public static DefenseBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<DefenseBuilding>(jsonString);
	}
}

[System.Serializable]
public class TrapBuilding : BuildingBase {
	public float damage;
	public float triggerRadius;
	public float damageRadius;
	public int reArmCost;
	public int damageType;
	public int unitTypeTarget;
	public int favoriteTarget;

	public static TrapBuilding CreateFromJSON(string jsonString){
		TrapBuilding t = JsonUtility.FromJson<TrapBuilding>(jsonString);
		t.maxHitpoint = 0;
		return t;
	}
}

[System.Serializable]
public class ResourceProducerBuilding : BuildingBase {
	public int boostCost;
	public int capacity;
	public int productionRate;
	
	public static ResourceProducerBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<ResourceProducerBuilding>(jsonString);
	}
}

[System.Serializable]
public class ResourceStorageBuilding : BuildingBase {
	public int storageCapacity;
	
	public static ResourceStorageBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<ResourceStorageBuilding>(jsonString);
	}
}

[System.Serializable]
public class TownHallBuilding : BuildingBase {
	/*
	BELUM
	*/
	public List<ResourceBuildingCapacity> resCap;
	public List<OffensiveBuildingCapacity> offCap;
	public List<DefensiveBuildingCapacity> defCap;

	public static TownHallBuilding CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<TownHallBuilding>(jsonString);
	}
}

[System.Serializable]
public class ResourceBuildingCapacity {
	public string namaFile;
	public int maxLvl;
	public int capacity;
}

[System.Serializable]
public class OffensiveBuildingCapacity {
	public string namaFile;
	public int maxLvl;
	public int capacity;
}

[System.Serializable]
public class DefensiveBuildingCapacity {
	public string namaFile;
	public int maxLvl;
	public int capacity;
}