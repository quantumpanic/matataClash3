using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildScript : MonoBehaviour
{
    public GameObject barrackPrefab;
    public GameObject goldMinePrefab;
    public GameObject campPrefab;
    public GameObject confirmUIPrefab;

    //public List<GameObject> buildingList;
    GameObject newBuildingPrefab;
    [SerializeField]
    GameObject nextBuilding;
    GameObject confirmUI;

    public static BuildScript Instance; 
    void Awake()
    {
        if (!Instance) Instance = this;
    }


    public void Build(string buildingName)
    {
        switch (buildingName)
        {
            case "Barrack":
                newBuildingPrefab = barrackPrefab;
                break;
            case "GoldMine":
                newBuildingPrefab = goldMinePrefab;
                break;
            case "Camp":
                newBuildingPrefab = campPrefab;
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
            SetEntityAvatar(gridScript.Instance.MakeBlueprint(5, 5), nextBuilding);
            SetConfirmButton();
        } else
            print("No worker available");
    }

/*
    void BuildBarrack()
    {
        if (GameManagerScript.Instance.GetWorker() > 0)
        {
            nextBuilding = (GameObject)Instantiate(barrackPrefab, new Vector3(2, 0.5f, 0), Quaternion.identity);
            SetConfirmButton();
        }
        else
            print("No worker available");

    }

    void BuildGoldMine()
    {
        if (GameManagerScript.Instance.GetWorker() > 0)
        {
            nextBuilding = (GameObject)Instantiate(goldMinePrefab, new Vector3(-1, 0.5f, -2), Quaternion.identity);
            SetEntityAvatar(gridScript.Instance.MakeEntity(5, 5), nextBuilding);
            SetConfirmButton();
        }
        else
            print("No worker available");

    }
*/

    public void BuildBlueprint(string buildingName)
    {
        // sebelum confirm build ada bayangan building

        switch (buildingName)
        {
            case "Barrack":
                nextBuilding = (GameObject)Instantiate(barrackPrefab, new Vector3(-1, 0.5f, -2), Quaternion.identity);
                SetEntityAvatar(gridScript.Instance.MakeEntity(5, 5), nextBuilding);
                SetConfirmButton();
                break;
            case "GoldMine":
                nextBuilding = (GameObject)Instantiate(goldMinePrefab, new Vector3(-1, 0.5f, -2), Quaternion.identity);
                SetEntityAvatar(gridScript.Instance.MakeBlueprint(5, 5), nextBuilding);
                SetConfirmButton();
                break;
        }
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
