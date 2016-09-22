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
        button.image.color = isDeployMode ? Color.red : new Color(255 / 255f, 245 / 255f, 210 / 255f);
        //if (isDeployMode) gridScript.Instance.LateGenerateNavMesh();
    }

    public GameObject combatUnit;
    public int unitDeployIndex;

    public GameObject DeployTroop(GridObject point)
    {
        // check ALL THE THINGS
        if (!isDeployMode) return null;
        if (TroopsManager.Instance.troops.Count < 1) return null;

        //TroopsManager.Instance.troops.RemoveRange(TroopsManager.Instance.troops.Count - 1, 1);
        //print(TroopsManager.Instance.troops.Count);
        //GameObject go = (GameObject)Instantiate(combatUnit, point.transform.position, Quaternion.identity);
        GameObject go = TroopsManager.Instance.troops[0];
        //unitDeployIndex++;
        go.SetActive(true);
        go.transform.position = point.transform.position;
        TroopsManager.Instance.troops.Remove(go);
        return go;
    }

    public void NewDamageEvent(DamageInstance dmg)
    {
        dmg.target.damageCalculator.ReceiveDamage(dmg);
    }

    public void NewDamageEvent(float dmg, IDamageable target)
    {
        target.damageCalculator.ReceiveDamage(dmg);
    }

    public void DamageReport(IDamageable target)
    {
        // confirm damage event worked correctly
    }
}

public interface IDamageable
{
    float curHP { get; set; }
    float maxHP { get; set; }
    GameObject body { get; set; }
    DamageCalculator damageCalculator { get; set; }
}

public interface IDamager
{
    float baseDmg { get; set; }
    CombatManager combatManager { get; }
}

public struct DamageInstance
{
    public IDamager owner;
    public IDamageable target;
    public DamageInstance(IDamager creator, IDamageable destination)
    {
        owner = creator;
        target = destination;
    }
}

public class DamageCalculator
{
    public IDamageable entity;
    public GameObject healthBar;

    public DamageCalculator(IDamageable creator)
    {
        entity = creator;
        healthBar = entity.body.transform.GetChild(1).GetChild(1).gameObject;
    }

    public void SetHealthVisual(float health)
    {
        if (!healthBar) return;
        healthBar.transform.localScale = new Vector3(health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    public void Die()
    {
        BuildingManager.Instance.buildingList.Remove(entity.body);
        GameObject.Destroy(entity.body);
    }

    public void ReceiveDamage(float dmg)
    {
        entity.curHP -= dmg;
        if (entity.curHP >= 0)
        {
            float a = (float)entity.curHP / entity.maxHP;
            SetHealthVisual(a);
            if (entity.curHP == 0) Die();

            // report dmg here
            CombatManager.Instance.DamageReport(entity);
        }
    }

    public void ReceiveDamage(DamageInstance dmgInst)
    {
        float dmg = dmgInst.owner.baseDmg;

        entity.curHP -= dmg;
        if (entity.curHP >= 0)
        {
            float a = (float)entity.curHP / entity.maxHP;
            SetHealthVisual(a);
            if (entity.curHP == 0) Die();

            // report dmg here
            CombatManager.Instance.DamageReport(entity);
        }
    }
}

public interface ITargettable
{
    GameObject body { get; set; }
    GridEntity entity { get; set; }
    TargetModule targetModule { get; set; }
}

public class TargetModule
{
    public ITargettable baseInterface;
    public RAIN.Entities.EntityRig entityRig;

    public TargetModule(ITargettable creator)
    {
        baseInterface = creator;
    }

    public void MakeTargetNodes()
    {
        Transform rigTransform = baseInterface.body.transform.GetChild(4);
        entityRig = rigTransform.GetComponent<RAIN.Entities.EntityRig>();
        foreach (GridObject go in baseInterface.entity.anchors)
        {
            TargetNode.MakeNode(baseInterface, rigTransform.gameObject, go.transform.position);
        }
    }
}

public interface IDamageableTarget : IDamageable, ITargettable { }

public class TargetNode : MonoBehaviour, IDamageableTarget
{
    public TargetModule targetModule { get; set; }
    public GridEntity entity { get { return targetModule.baseInterface.entity; } set { } }
    public GameObject body { get { return targetModule.baseInterface.body; } set { } }
    public float curHP { get { return targetModule.baseInterface.body.GetComponent<BuildingScript>().curHP; } set { } }
    public float maxHP { get { return targetModule.baseInterface.body.GetComponent<BuildingScript>().maxHP; } set { } }
    public DamageCalculator damageCalculator { get { return targetModule.baseInterface.body.GetComponent<BuildingScript>().damageCalculator; } set { } }

    public static TargetNode MakeNode(ITargettable baseInterface, GameObject baseObj, Vector3 pos)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(baseObj, pos, Quaternion.identity);
        if (baseObj.transform.parent.name.Contains("Wall")) return null;
        obj.GetComponent<RAIN.Entities.EntityRig>().Entity.Form = obj;
        obj.GetComponent<RAIN.Entities.EntityRig>().Entity.GetAspect("BuildingVisualAspect").MountPoint = obj.transform;

        TargetNode node = obj.AddComponent<TargetNode>();
        node.targetModule = baseInterface.targetModule;
        obj.transform.parent = node.entity.avatar.transform;

        return node;
    }
}