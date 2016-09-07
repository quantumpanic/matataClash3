using UnityEngine;
using System.Collections;
using RAIN;

public class DetectionPriority : MonoBehaviour
{

    RAIN.Perception.Sensors.RAINSensor sensor;
    RAIN.Core.AI ai;

    // Use this for initialization
    void Start()
    {
    }

	float closestDist;

    void DetectClosestAspect(string aspectName)
    {
        sensor = ai.Senses.GetSensor(aspectName);
        sensor.Sense(aspectName, RAIN.Perception.Sensors.VisualSensor.MatchType.ALL);

        foreach (RAIN.Entities.Aspects.RAINAspect aspect in sensor.Matches)
        {
            // set nearest target
			float aspectDist = Vector3.Distance(sensor.Position, aspect.Position);
			if (aspectDist > closestDist) closestDist = aspectDist;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
