using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour
{

    public static CombatManager Instance;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isDeployMode;

    public void ToggleDeployMode(UnityEngine.UI.Button button)
    {
        isDeployMode = !isDeployMode;
        button.image.color = isDeployMode ? Color.red : new Color(255/255f, 245/255f, 210/255f);
    }

    public GameObject combatUnit;

    public GameObject DeployTroop(GridObject point)
    {
        if (!isDeployMode) return null;
        GameObject go = (GameObject)Instantiate(combatUnit, point.transform.position, Quaternion.identity);
        return go;
    }
}
