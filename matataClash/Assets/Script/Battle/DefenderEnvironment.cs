using UnityEngine;
using System.Collections;
using RAIN.Navigation.NavMesh;
using System.Threading;
using System;

public class DefenderEnvironment : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GenerateNavmesh();
	}

    private void GenerateNavmesh()
    {
        NavMeshRig rig = GetComponent<NavMeshRig>();
		rig.NavMesh.UnregisterNavigationGraph();
		rig.NavMesh.StartCreatingContours(rig, 4);
		while (rig.NavMesh.Creating){
			rig.NavMesh.CreateContours();
			Thread.Sleep(10);
		}
		rig.NavMesh.RegisterNavigationGraph();
    }

}
