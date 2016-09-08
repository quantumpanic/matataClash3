using UnityEngine;
using System.Collections;

public class TroopScript : MonoBehaviour, IDamager, IDamageable
{
    public float maxHitpoint;
    public float curHitpoint;
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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExecuteAttack()
    {
        // called after finish attack animation
        // use event

        GameObject target = GetComponent<DetectionPriority>().aiRig.AI.WorkingMemory.GetItem<GameObject>("targetPos");
        IDamageable targetScript = target.GetComponent<IDamageable>();

        combatManager.NewDamageEvent(new DamageInstance(this, targetScript));
    }
}
