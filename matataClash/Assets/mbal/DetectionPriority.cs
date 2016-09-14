
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
		ResetMemory(true);
        InvokeRepeating("CheckMemory", 0, .5f);
    }

    public float closestDist;

    void DetectClosestAspect(string aspectName)
    {
        sensor = aiRig.AI.Senses.GetSensor("KnightVisual");
        sensor.Sense(aspectName, RAIN.Perception.Sensors.VisualSensor.MatchType.ALL);
		GameObject nearestTarget = null;
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
        doDetectClosest = aiRig.AI.WorkingMemory.GetItem<bool>("chkDist");
    }

    // Update is called once per frame
    void Update()
    {
        if (doDetectClosest) DetectClosestAspect("BuildingVisualAspect");
    }
}