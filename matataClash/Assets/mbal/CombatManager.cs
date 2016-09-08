﻿using UnityEngine;
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
    DamageCalculator damageCalculator { get; set; }
}

public interface IDamager
{
    float baseDmg { get; set; }
    CombatManager combatManager { get; }
}

public class DamageInstance
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
        }
    }
}