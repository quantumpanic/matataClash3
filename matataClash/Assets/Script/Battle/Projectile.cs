using UnityEngine;
using System.Collections;
using System;

public class Projectile : ProjectileAbstract
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Projectile Create()
    {
        GameObject go = new GameObject();
        Projectile projectile = go.AddComponent<Projectile>();
        return projectile;
    }

    public void Initialize(IDamager creator = null)
    {
        origin = creator;
        if (origin == null) origin = this;
        baseDmg = origin.baseDmg;
    }

    public void CenterToOrigin()
    {
        gameObject.transform.position = origin.body.transform.position;
    }

    public float travelSpeed;
    public float lifeTime;
    private float timeAlive;
    public float maxDist;
    private float distTraveled;

    public IDamager origin;

    public void MoveForward()
    {
        transform.Translate(Vector3.forward * travelSpeed);
    }

    public void SetTarget(IDamageable target)
    {

    }
}

public abstract class ProjectileAbstract : MonoBehaviour, IDamager
{
    // abstract classes are here so we don't need to look at interface implementations

    public float baseDmg
    {
        get;
        set;
    }

    public GameObject body
    {
        get;
        set;
    }

    public CombatManager combatManager
    {
        get
        {
            return CombatManager.Instance;
        }
    }
}