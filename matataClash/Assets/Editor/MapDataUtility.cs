using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class MapDataUtility : ScriptableObject {

    // map where all entities are
    public List<MapEntity> mapEntities = new List<MapEntity>();

    [MenuItem("Assets/Create/New Map Data Asset")]
    public static MapData CreateMapDataAsset()
    {
        MapData asset = ScriptableObject.CreateInstance<MapData>();

        AssetDatabase.CreateAsset(asset, "Assets/mbal/MapData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        return asset;
    }

    [MenuItem("Assets/Create/Map Data From Scene")]
    public static MapData CreateMapDataFromScene()
    {
        MapData md = CreateMapDataAsset();
        foreach (GridEntity ge in gridScript.Instance.entities)
        {
            md.mapEntities.Add(MapEntity.CreateFromGrid(ge));
        }

        return md;
    }
}
