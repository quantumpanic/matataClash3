using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
		CheckNeighbor();
	}
	
	// Update is called once per frame
	void Update () {
		CheckNeighbor();
	}

	void CheckNeighbor(){
		GridEntity ge = gameObject.GetComponent<BuildingScript>().entity;
		GridObject go = gridScript.Instance.StrictTileLookup(ge.MainAnchor, 1 , 1);
		GridObject top = gridScript.Instance.StrictTileLookup(go, 0, 3);
		if(top) print(top.name);
		GridObject bottom = gridScript.Instance.StrictTileLookup(go, 0, -3);
		if(bottom) print(bottom.name);
		GridObject right = gridScript.Instance.StrictTileLookup(go, 3, 0);
		if(right) print(right.name);
		GridObject left = gridScript.Instance.StrictTileLookup(go, -3, 0);
		if(left) print(left.name);
	}
}
