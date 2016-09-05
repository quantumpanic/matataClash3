using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildScript : MonoBehaviour
{
    public GameObject barrackPrefab;
    public GameObject goldMinePrefab;
    public GameObject campPrefab;
    public GameObject wallPrefab;
    public GameObject confirmUIPrefab;

    //public List<GameObject> buildingList;
    GameObject newBuildingPrefab;
    [SerializeField]
    GameObject nextBuilding;
    GameObject confirmUI;
    int buildingSize;
    string buildingName;
    int category;//1 army, 2 def, 3 res

    //public Dictionary<int, string> buildingDict = new Dictionary<int, string>();

    public static BuildScript Instance; 
    void Awake()
    {
        if (!Instance) Instance = this;

        //GoldMine
        //buildingDict.Add(1, "resBuilding");
        //Camp
       // buildingDict.Add(1, "armyCamp");
        //
    }


    public void Build(string buildingName)
    {
        this.buildingName = buildingName;

        switch (buildingName)
        {
            case "Barrack":
                newBuildingPrefab = barrackPrefab;
                buildingSize = 5;
                category = 0;
                break;
            case "GoldMine":
                newBuildingPrefab = goldMinePrefab;
                buildingSize = 5;
                category = 3;
                break;
            case "Camp":
                newBuildingPrefab = campPrefab;
                buildingSize = 5;
                category = 1;
                break;
            case "Wall":
                newBuildingPrefab = wallPrefab;
                buildingSize = 3;
                category = 2;
                break;
        }
        if (CancelBuild()) return;
        else InstanceBuild();
    }

    void InstanceBuild()
    {
        if (GameManagerScript.Instance.GetWorker() > 0)
        {
            nextBuilding = (GameObject)Instantiate(newBuildingPrefab, new Vector3(2, 0.5f, 0), Quaternion.identity);
            SetEntityAvatar(gridScript.Instance.MakeBlueprint(buildingSize, buildingSize), nextBuilding);
            SetConfirmButton();
        } else
            print("No worker available");
    }


    public void SetEntityAvatar(GridEntity ge, GameObject go)
    {
        ge.avatar = go;
        inputManager.Instance.selectedEntity = ge;

        go.GetComponent<BuildingScript>().entity = ge;
    }

    void SetConfirmButton()
    {
        confirmUI = (GameObject)Instantiate(confirmUIPrefab, nextBuilding.transform.position, Quaternion.Euler(0, 45, 0));
        confirmUI.transform.SetParent(nextBuilding.transform);
        confirmUI.transform.localPosition = Vector3.zero;

        Button okButton = confirmUI.transform.GetChild(0).GetComponent<Button>();
        Button cancelButton = confirmUI.transform.GetChild(1).GetComponent<Button>();

        okButton.onClick.AddListener(() => ConfirmBuild());
        cancelButton.onClick.AddListener(() => CancelBuild());
    }

    void ConfirmBuild()
    {
        if (!inputManager.Instance.selectedEntity.Root()) return;
        //Destroy(confirmUI);
        confirmUI.SetActive(false);
        nextBuilding.GetComponent<BuildingScript>().Build();
        //nextBuilding.GetComponent<BuildingScript>().Build(buildingName, category);
        GameManagerScript.Instance.SetWorker(-1);
        BuildingManager.Instance.addBuilding(nextBuilding);
        nextBuilding = null;
    }

    bool CancelBuild()
    {
        if (nextBuilding)
        {
            Destroy(nextBuilding);
            return true;
        }

        return false;
    }
}
