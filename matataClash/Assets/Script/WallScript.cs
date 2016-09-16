using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	public GameObject upWall;
	public GameObject downWall;
	public GameObject rightWall;
	public GameObject leftWall;
	public bool doLink = true;
	// Use this for initialization
	void Update(){
		if (doLink)
			CheckNeighbor();
	}
	public void CheckNeighbor(){
		GridEntity ge = gameObject.GetComponent<BuildingScript>().entity;
		GridObject go = gridScript.Instance.StrictTileLookup(ge.MainAnchor, 1 , 1);
		GridObject top = gridScript.Instance.StrictTileLookup(go, 0, 2);
		if(top && top.entity){
			if(top.entity.GetComponent<GridEntity>().avatar.GetComponent<BuildingScript>().buildingType == 4){
				upWall.SetActive(true);
				top.entity.GetComponent<GridEntity>().avatar.GetComponent<WallScript>().Adapt(2);
			}
		}else
			upWall.SetActive(false);
		//if(top) print(top.name);
		GridObject bottom = gridScript.Instance.StrictTileLookup(go, 0, -2);
		if(bottom && bottom.entity){
			if(bottom.entity.GetComponent<GridEntity>().avatar.GetComponent<BuildingScript>().buildingType == 4){
				downWall.SetActive(true);
				bottom.entity.GetComponent<GridEntity>().avatar.GetComponent<WallScript>().Adapt(1);
			}
		}else
			downWall.SetActive(false);
		//if(bottom) print(bottom.name);
		GridObject right = gridScript.Instance.StrictTileLookup(go, 2, 0);
		if(right && right.entity){
			if(right.entity.GetComponent<GridEntity>().avatar.GetComponent<BuildingScript>().buildingType == 4){
				rightWall.SetActive(true);
				right.entity.GetComponent<GridEntity>().avatar.GetComponent<WallScript>().Adapt(4);
			}
		}else
			rightWall.SetActive(false);
		//if(right) print(right.name);
		GridObject left = gridScript.Instance.StrictTileLookup(go, -2, 0);
		if(left && left.entity){
			if(left.entity.GetComponent<GridEntity>().avatar.GetComponent<BuildingScript>().buildingType == 4){
				leftWall.SetActive(true);
				left.entity.GetComponent<GridEntity>().avatar.GetComponent<WallScript>().Adapt(3);
			}
		}else
			leftWall.SetActive(false);
		//if(left) print(left.name);
	}

	public void Adapt(int w, bool a = true){
		switch(w){
			case 1: 
				upWall.SetActive(a);
				break;
			case 2: 
				downWall.SetActive(a);
				break;
			case 3: 
				rightWall.SetActive(a);
				break;
			case 4: 
				leftWall.SetActive(a);
				break;
			

		}
	}

}
