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
            gridScript.Instance.MakeEntity(5, 5, ent.Index[0], ent.Index[1]);
            // add the avatar
        }
    }

    public void LoadDefaultMap()
    {
        LoadMapLayout(SceneManager.Instance.defaultScene.mapDataFile);
    }

    public void ClearMap()
    {
        return;
        List<MonoBehaviour> tempList = new List<MonoBehaviour>();
        foreach (GridEntity ge in gridScript.Instance.entities)
        {
            if (ge) tempList.Add(ge);
            Destroy(tempList[0]);
        }

        gridScript.Instance.entities.Clear();
        print("clear");
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
        MapEntity me = new MapEntity();
        me.Index = ge.Index;
        me.entityID = 0;

        return me;
    }
}