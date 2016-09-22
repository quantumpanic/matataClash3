using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

public class MapManager : MonoBehaviour
{

    public static MapManager Instance;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        LoadDefaultMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GenerateNavMesh()
    {
        yield return new WaitForEndOfFrame();
        DefenderEnvironment.Instance.GenerateNavmesh();
    }

    public void LoadMapLayout(MapData data)
    {
        ClearMap();

        if (!data) return;

        foreach (MapEntity ent in data.mapEntities)
        {
            //gridScript.Instance.MakeEntity(5, 5, ent.Index[0], ent.Index[1]);

            GameObject buildingType = null;
            switch (ent.entityID)
            {
                case 0:
                    buildingType = BuildScript.Instance.townHallPrefab;
                    break;
                case 1:
                    buildingType = BuildScript.Instance.barrackPrefab;
                    break;
                case 2:
                    buildingType = BuildScript.Instance.campPrefab;
                    break;
                case 3:
                    buildingType = BuildScript.Instance.goldMinePrefab;
                    break;
                case 4:
                    buildingType = BuildScript.Instance.wallPrefab;
                    break;
                case 5:
                    buildingType = BuildScript.Instance.manaPrefab;
                    break;
                default:
                    break;
            }

            // add the avatar
            if (buildingType == null) continue;
            GameObject building = (GameObject)Instantiate(buildingType, Vector3.zero, Quaternion.identity);
            //BuildingManager.Instance.addBuilding(building);
            BuildingScript script = building.GetComponent<BuildingScript>();
            script.xPos = ent.Index[0];
            script.yPos = ent.Index[1];
            script.isBuiltBeforeStart = true;
        }

        if (SceneManager.Instance.isCombatMap)
        {
            gridScript.Instance.LateGenerateNavMesh();
            CombatManager.Instance.isDeployMode = false;
        }
    }

    public void LoadDefaultMap()
    {
        LoadMapLayout(SceneManager.Instance.defaultScene.mapDataFile);
    }

    public void ClearMap()
    {
        // clear buildings
        List<MonoBehaviour> tempList = new List<MonoBehaviour>();
        foreach (GridEntity ge in gridScript.Instance.entities)
        {
            if (ge) tempList.Add(ge);
        }

        foreach (GridEntity ge in tempList)
        {
            Destroy(ge);
        }

        // clear units
        List<GameObject> tempGoList = new List<GameObject>();
        foreach (GameObject go in TroopsManager.Instance.survivingTroops)
        {
            if (go) tempGoList.Add(go);
        }

        foreach (GameObject go in tempGoList)
        {
            go.transform.GetChild(0).GetComponent<TroopScript>().Evacuate();
        }

        // clear camplist before spawning new camps
        TroopsManager.Instance.campList.Clear();
        // reset the unit counter
        CombatManager.Instance.unitDeployIndex = 0;

        gridScript.Instance.entities.Clear();
        StartCoroutine(RefreshGrid());
    }

    IEnumerator RefreshGrid()
    {
        yield return new WaitForEndOfFrame();
        gridScript.Instance.UpdateGridCursor();
    }
}

[System.Serializable]
public class MapEntity
{
    // entity that holds information of unit/building
    public int[] Index = new int[2];
    public int entityID;

    public static MapEntity CreateFromGrid(GridEntity ge)
    {
        if(!ge.avatar) return null;
        MapEntity me = new MapEntity();
        me.Index = ge.Index;
        me.entityID = ge.avatar.GetComponent<BuildingScript>().buildingType;

        return me;
    }
}