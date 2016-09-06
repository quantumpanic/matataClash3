using UnityEngine;
using System.Collections.Generic;


public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    public List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            var g = Instantiate((GameObject)Instantiate(gridScript.Instance.tileObj, Vector3.zero, Quaternion.identity));
            pool.Add(g);
        }
    }

    public GameObject GetFromPool()
    {
        if (pool.Count > 0)
        {
            var p = pool[0];
            pool.RemoveAt(0);
            return p;
        }

        return null;
    }

    public void ReturnToPool(GameObject g)
    {
        pool.Add(g);
    }
}
