using UnityEngine;
using System.Collections;

public class RangedTroopScript : TroopScript {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    new public void ExecuteAttack()
    {
        // called after finish attack animation
        // use event

        GameObject target = GetComponent<DetectionPriority>().aiRig.AI.WorkingMemory.GetItem<GameObject>("targetPos");
        IDamageable targetScript = target.GetComponent<IDamageableTarget>();

        // check if in range before dealing damage
        if (GetComponent<DetectionPriority>().closestDist < 3f)
        {
			Projectile p = Projectile.Create().Initialize(this);
			p.CenterToOrigin();
			p.SetTarget(targetScript);
            combatManager.NewDamageEvent(new DamageInstance(p, targetScript));
        }
    }
}