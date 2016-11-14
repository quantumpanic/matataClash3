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
        if (currentTarget) transform.LookAt(currentTarget.transform);
        MoveForward();
    }

    public static Projectile Create()
    {
        GameObject go = new GameObject();
        Projectile projectile = go.AddComponent<Projectile>();
        return projectile;
    }

    public Projectile Initialize(IDamager creator = null)
    {
        origin = creator;
        if (origin == null) origin = this;
        baseDmg = origin.baseDmg;

        return this;
    }

    public void CenterToOrigin()
    {
        gameObject.transform.position = origin.body.transform.position;
    }

    private float travelSpeed;
    public float lifeTime;
    private float timeAlive;
    public float maxDist;
    private float distTraveled;

    public IDamager origin;

    public void MoveForward()
    {
        transform.Translate(Vector3.forward * travelSpeed);
    }

    private GameObject currentTarget;

    public void SetTarget(IDamageable target, float speed = -1)
    {
        currentTarget = target.body;
        if (!(speed < 0)) travelSpeed = speed;
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