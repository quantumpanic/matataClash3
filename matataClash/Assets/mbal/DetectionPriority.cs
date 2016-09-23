
using UnityEngine;
using System.Collections;

public class DetectionPriority : MonoBehaviour
{

    RAIN.Perception.Sensors.RAINSensor sensor;
    public RAIN.Core.AIRig aiRig;
    bool doDetectClosest;

    // Use this for initialization
    void Start()
    {
		aiRig.AI.WorkingMemory.SetItem<GameObject>("thisObj", aiRig.gameObject);
		ResetMemory(true);
        InvokeRepeating("CheckMemory", 0, .1f);
    }

    public float closestDist;

    void DetectClosestAspect(string aspectName)
    {
        sensor = aiRig.AI.Senses.GetSensor("KnightVisual");
        sensor.Sense(aspectName, RAIN.Perception.Sensors.VisualSensor.MatchType.ALL);
		GameObject nearestTarget = aiRig.gameObject;
		closestDist = 999999;

        foreach (RAIN.Entities.Aspects.RAINAspect aspect in sensor.Matches)
        {
            // set nearest target
            float aspectDist = Vector3.Distance(sensor.Position, aspect.Position);
            if (aspectDist < closestDist)
            {
                closestDist = aspectDist;
				nearestTarget = aspect.Entity.Form;
            }
        }

		aiRig.AI.WorkingMemory.SetItem<GameObject>("targetPos", nearestTarget);
		ResetMemory(false);
    }

	void ResetMemory(bool active)
	{
        doDetectClosest = active;
        aiRig.AI.WorkingMemory.SetItem<bool>("chkDist", active);
	}

    void CheckMemory()
    {
		aiRig.AI.WorkingMemory.SetItem<GameObject>("targetPos", null);
        doDetectClosest = aiRig.AI.WorkingMemory.GetItem<bool>("chkDist");
        DetectClosestAspect("BuildingVisualAspect");
    }

    // Update is called once per frame
    void Update()
    {
        if (doDetectClosest) DetectClosestAspect("BuildingVisualAspect");
    }
}