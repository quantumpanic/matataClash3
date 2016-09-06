using UnityEngine;
using System.Collections;
using RAIN.Navigation.NavMesh;
using System.Threading;
using System;

public class DefenderEnvironment : MonoBehaviour {

	public static DefenderEnvironment Instance;

	void Awake()
	{
		if (!Instance) Instance = this;
	}

	// Use this for initialization
	void Start () {
		GenerateNavmesh();
	}

    public void GenerateNavmesh()
    {
        NavMeshRig rig = GetComponent<NavMeshRig>();
		rig.NavMesh.UnregisterNavigationGraph();
		rig.NavMesh.StartCreatingContours(1);
		while (rig.NavMesh.Creating){
			rig.NavMesh.CreateContours();
			Thread.Sleep(60);
		}
		rig.NavMesh.RegisterNavigationGraph();
    }

}
