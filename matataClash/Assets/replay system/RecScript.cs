using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecScript : MonoBehaviour
{
    bool isRec = false;
    bool doPlay = false;
    List<float> nums = new List<float>();
    List<float> instans = new List<float>();
    float tempX;
    float tempY;
    float tempZ;
    bool playedNoRep = false;
    public Vector3 startPosi;
    
    // Use this for initialization
    void Start()
    {

    }

    public void Record()
    {
        if (isRec == true)
        {
            isRec = false;
            transform.position = startPosi;
        }
        else if (isRec == false)
        {
            //GetComponent<Camera>().enabled = true;
            startPosi = transform.position;
            isRec = true;
            doPlay = false;
        }
    }
	
    // Update is called once per frame
    public void Update()
    {
        if (playedNoRep == true)
        {
            doPlay = false;
        }

        if (isRec == true)
        {
            tempX = transform.position.x;
            tempY = transform.position.y;
            tempZ = transform.position.z;
            nums.Add(tempX);
            nums.Add(tempY);
            nums.Add(tempZ);
        }

        if (doPlay == true)
        {

            doPlay = false;
            StartCoroutine("Playback");
            Debug.Log(doPlay);
        }
    }


    public IEnumerator Playback()
    {

        playedNoRep = true;
        for (int i = 0; i < nums.Count; i += 3)
        {
            transform.position = new Vector3(nums[i], nums[i + 1], nums[i + 2]);
            yield return null;
        }
    }

    public void Reset()
    {
        nums.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene("SciFi Level");
    }
    public void RunIt()
    {
        isRec = false;
        doPlay = true;
        StartCoroutine("Playback");
    }
    public void showThisCam()
    {
        if (doPlay == true)
        {
            GetComponent<Camera>().enabled = true;
        }
    }
}
