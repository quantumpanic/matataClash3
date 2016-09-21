using UnityEngine;
using System.Collections;

public class TroopScript : MonoBehaviour, IDamager, IDamageable
{
    public float maxHitpoint;
    public float curHitpoint;
    public int campIndex;
    public float maxHP
    {
        get
        {
            return maxHitpoint;
        }
        set
        {
            maxHitpoint = value;
        }
    }
    public float curHP
    {
        get
        {
            return curHitpoint;
        }
        set
        {
            curHitpoint = value;
        }
    }
    public GameObject body
    {
        get
        {
            return gameObject;
        }
        set
        {

        }
    }
    public float baseDamage;
    public float baseDmg
    {
        get
        {
            return baseDamage;
        }
        set
        {
            baseDamage = value;
        }
    }
    public float attackSpeed;

    public DamageCalculator damageCalculator { get; set; }
    public CombatManager combatManager { get { return CombatManager.Instance; } }

    // Use this for initialization
    void Start()
    {
        Invoke("Suicide", 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Suicide()
    {
        Destroy(transform.parent.gameObject);
    }

    void OnDestroy()
    {
        TroopsManager.Instance.survivingTroops.Remove(transform.parent.gameObject);
    }

    public void ExecuteAttack()
    {
        // called after finish attack animation
        // use event

        GameObject target = GetComponent<DetectionPriority>().aiRig.AI.WorkingMemory.GetItem<GameObject>("targetPos");
        IDamageable targetScript = target.GetComponent<IDamageableTarget>();

        // check if in range before dealing damage
        if (GetComponent<DetectionPriority>().closestDist < 0.3f)
        {
            combatManager.NewDamageEvent(new DamageInstance(this, targetScript));
        }
    }
}
