using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class BuildingScript : MonoBehaviour, IDamageableTarget
{

    //public Button upgradeButton;
    public Text upgradeTimeText;
    GameObject uiCanvas;

    [SerializeField]
    int level;
    public float buildTime;
    public float upgradeTime;
    [SerializeField]
    float timeLeft;
    public bool isBuilding;
    public bool isUpgrading;
    string minutes;
    string seconds;
    public int resourceNeeded;
    //public int buildingType;
    public bool isBuiltBeforeStart;
    public int size;
    public int xPos;
    public int yPos;
    public float maxHitpoint;
    public float curHitpoint;
    public GridEntity entity { get; set; }
    public TargetModule targetModule { get; set; }
    //public BuildingData buidingData
    [HideInInspector]
    public enum BuildingType {TownHall, Barrack, Camp, GoldMine, ManaGenerator, Wall};
    public BuildingType buildingType;
    //public BuildingCategory buildingCategory;
    public BuildingBase buildingBase;


    // Use this for initialization
    void Awake()
    {
        level = 1;
        isBuilding = false;
        isUpgrading = false;
        curHitpoint = maxHitpoint;
        uiCanvas = transform.GetChild(0).gameObject;

        damageCalculator = new DamageCalculator(this);

        //buidingData = new BuildingData(1);
        ReadDataBuilding();        
    }

    public float curHP    {
        get
        {
            return curHitpoint;
        }
        set
        {
            curHitpoint = value;
        }
    }
    public float maxHP    {
        get
        {
            return maxHitpoint;
        }
        set
        {
            maxHitpoint = value;
        }
    }

    public GameObject body    {
        get
        {
            return gameObject;
        }
        set
        {

        }
    }
    public DamageCalculator damageCalculator { get; set; }

    void Start()
    {
        
        if (isBuiltBeforeStart)
        {   
            //print("start");
            var ge = gridScript.Instance.MakeEntity(size, size, xPos, yPos);
            BuildScript.Instance.SetEntityAvatar(ge, gameObject);
            BuildingManager.Instance.addBuilding(gameObject);
      //      BuildingManager.z++;
      //      print(BuildingManager.z);

            switch (buildingType) {
                case BuildingType.Camp:
                    TroopsManager.Instance.addCamp(gameObject);
                    break;
                case BuildingType.GoldMine:
                case BuildingType.ManaGenerator:
                    //
                    if(!SceneManager.Instance.isCombatMap)
                        gameObject.GetComponent<ResourceCollectorScript>().ProduceResources();
                    break;
                default:
                    break;
            }
                
        }

        // add target module
        targetModule = new TargetModule(this);

        // add entity destroyer
        destroyEvt += entity.AvatarHandler;

        // show hp bar
        if (!SceneManager.Instance.isCombatMap) transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //not building and  selected
        if(!SceneManager.Instance.isCombatMap) {
            if (!isBuilding && (inputManager.Instance.selectedEntity.avatar == gameObject) && !inputManager.Instance.selectedEntity.isBlueprint)
            {
                uiCanvas.gameObject.SetActive(true);
            } else {
                uiCanvas.gameObject.SetActive(false);
            }
        }

        if (isBuilding)
        {
            SetTime("Build");
            if (timeLeft <= 0)
            {
                upgradeTimeText.gameObject.SetActive(false);
                GameManagerScript.Instance.SetWorker(1);
                isBuilding = false;

                if (this.GetComponent<ResourceCollectorScript>())
                    this.GetComponent<ResourceCollectorScript>().ProduceResources();
                if (this.GetComponent<CampScript>())
                    TroopsManager.Instance.addCamp(this.gameObject);
                if (this.GetComponent<WallScript>())
                    this.GetComponent<WallScript>().CheckNeighbor();
            }

        }

        if (isUpgrading)
        {
            uiCanvas.gameObject.SetActive(false);
            SetTime("Upgrade");
            if (timeLeft <= 0)
            {
                level++;
                upgradeTimeText.gameObject.SetActive(false);
                GameManagerScript.Instance.SetWorker(1);
                ReadDataBuilding();
                isUpgrading = false;
                print("upgraded to " + level);


            }
        }

    }

    public void Upgrade()
    {
        if (!isUpgrading && GameManagerScript.Instance.GetWorker() > 0)
        {
            timeLeft = upgradeTime;
            GameManagerScript.Instance.SetWorker(-1);
            isUpgrading = true;
        }
        else
            //print ("boo");
            TextAnimManager.Instance.WarningNoWorker();



        //		isSelected = false;
        //		upgradeButton.gameObject.SetActive (false);
    }

    public void Build()
    {
        timeLeft = buildTime;
        isBuilding = true;
    }

    void SetTime(string status)
    {
        minutes = Mathf.Floor(timeLeft / 60).ToString("00");
        seconds = (timeLeft % 60).ToString("00");
        upgradeTimeText.gameObject.SetActive(true);
        upgradeTimeText.text = status + " \n " + minutes + " min " + seconds + " sec";
        timeLeft -= Time.deltaTime;
    }

    void ReadDataBuilding(){
        switch(buildingType){
            case BuildingType.Barrack:
                //Barrack
                BuildingBase data = DataReader.Instance.armyProducerBuildingList.Find(obj=>(obj.name=="Barracks" && obj.level==level));
                buildingBase = ObjectCopier.Clone(data);
                break;
        }
    }

    public delegate void DestroyEvent(GameObject g);
    public event DestroyEvent destroyEvt;

    public void OnDestroy()
    {
        BuildingManager.Instance.buildingList.Remove(gameObject);
     //   BuildingManager.z =0;
        // trigger for entity destroy
        if (destroyEvt != null) destroyEvt(gameObject);
    }

}
