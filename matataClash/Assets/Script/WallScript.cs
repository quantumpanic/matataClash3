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
		GameObject obj;

		GridObject top = gridScript.Instance.StrictTileLookup(go, 0, 2);		
		if(top && top.entity){
			obj = top.entity.GetComponent<GridEntity>().avatar;
			if(obj.GetComponent<BuildingScript>().buildingType == BuildingScript.BuildingType.Wall){
				upWall.SetActive(true);
				obj.GetComponent<WallScript>().Adapt(2);
			}
		}else
			upWall.SetActive(false);

		GridObject bottom = gridScript.Instance.StrictTileLookup(go, 0, -2);
		if(bottom && bottom.entity){
			obj = bottom.entity.GetComponent<GridEntity>().avatar;
			if(obj.GetComponent<BuildingScript>().buildingType == BuildingScript.BuildingType.Wall){
				downWall.SetActive(true);
				obj.GetComponent<WallScript>().Adapt(1);
			}
		}else
			downWall.SetActive(false);

		GridObject right = gridScript.Instance.StrictTileLookup(go, 2, 0);
		if(right && right.entity){
			obj = right.entity.GetComponent<GridEntity>().avatar;
			if(obj.GetComponent<BuildingScript>().buildingType == BuildingScript.BuildingType.Wall){
				rightWall.SetActive(true);
				obj.GetComponent<WallScript>().Adapt(4);
			}
		}else
			rightWall.SetActive(false);

		GridObject left = gridScript.Instance.StrictTileLookup(go, -2, 0);
		if(left && left.entity){
			obj = left.entity.GetComponent<GridEntity>().avatar;
			if(obj.GetComponent<BuildingScript>().buildingType == BuildingScript.BuildingType.Wall){
				leftWall.SetActive(true);
				obj.GetComponent<WallScript>().Adapt(3);
			}
		}else
			leftWall.SetActive(false);
			
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
