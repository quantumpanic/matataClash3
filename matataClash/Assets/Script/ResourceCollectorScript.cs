using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceCollectorScript : MonoBehaviour
{

    public int resGathered;
    public int maxResGathered;
    public float gatherTime;
    float currentTime;
    float elapsedTime = 0;
    bool isGathering = false;
    public Button collectButton;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isGathering && resGathered < maxResGathered)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= gatherTime)
            {
                resGathered += 50;
                collectButton.gameObject.SetActive(true);
                elapsedTime = 0;
            }
        }
        else
            isGathering = false;

        if (resGathered >= maxResGathered)
        {
            //print ("Res full");
            //GetComponent<Renderer>().material.color = Color.red;
        }
        else
		{
            //GetComponent<Renderer>().material.color = Color.yellow;
		}
    }

    public void ProduceResources()
    {
        resGathered = 0;
        currentTime = Time.time;
        isGathering = true;
    }

    public void CollectResources()
    {
        GameManagerScript.Instance.SetGold(resGathered);
        TextAnimManager.Instance.SpawnAddGoldText(transform.position, resGathered.ToString());
        resGathered = 0;
        ProduceResources();
        collectButton.gameObject.SetActive(false);
    }
}
