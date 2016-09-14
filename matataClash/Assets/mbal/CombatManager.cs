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
    }

    public GameObject combatUnit;

    public GameObject DeployTroop(GridObject point)
    {
        if (!isDeployMode) return null;
        GameObject go = (GameObject)Instantiate(combatUnit, point.transform.position, Quaternion.identity);
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

    public void ReceiveDamage(float dmg)
    {
        entity.curHP -= dmg;
        if (entity.curHP >= 0)
        {
            float a = (float)entity.curHP / entity.maxHP;
            SetHealthVisual(a);

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

            // report dmg here
            CombatManager.Instance.DamageReport(entity);
        }
    }
}

public interface ITargettable
{
    GridEntity entity { get; set; }
    TargetModule targetModule { get; set; }
}

public class TargetModule
{
    public GridEntity entity;
    public RAIN.Entities.EntityRig entityRigBase;

    public TargetModule(GridEntity creator)
    {
        entity = creator;
        Debug.Log(entity);
    }

    public void MakeTargetNodes()
    {
        Transform rigTransform = entity.avatar.transform.GetChild(4);
        entityRigBase = rigTransform.GetComponent<RAIN.Entities.EntityRig>();
        foreach (GridObject go in entity.anchors)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(entityRigBase.gameObject, go.transform.position, Quaternion.identity);
            obj.GetComponent<RAIN.Entities.EntityRig>().Entity.Form = obj;
            obj.GetComponent<RAIN.Entities.EntityRig>().Entity.GetAspect("BuildingVisualAspect").MountPoint = obj.transform;
            obj.transform.parent = entity.avatar.transform;
        }
    }
}