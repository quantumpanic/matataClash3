using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingScript : MonoBehaviour {

	//public Button upgradeButton;
	public Text upgradeTimeText;
	GameObject uiCanvas;

	[SerializeField]
	int level;
	public float buildTime;
	public float upgradeTime;
	[SerializeField]
	float timeLeft;
	public bool isBuilding;
	public bool isUpgrading;
	string minutes;
	string seconds;
	public int resourceNeeded;
	public int buildingType;
	public bool isBuiltBeforeStart;
	public int size;
	public int xPos;
	public int yPos;
	public GridEntity entity;

	// Use this for initialization
	void Awake () {
		level = 1;
		isBuilding = false;
		isUpgrading = false;

		uiCanvas = transform.GetChild(0).gameObject;
	}

	void Start () {
		if (isBuiltBeforeStart) {
			var ge = gridScript.Instance.MakeEntity(size, size, xPos, yPos);
			BuildScript.Instance.SetEntityAvatar(ge, gameObject);
			BuildingManager.Instance.addBuilding(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//not building and  selected
		if (!isBuilding && (inputManager.Instance.selectedEntity.avatar == gameObject) && !inputManager.Instance.selectedEntity.isBlueprint ) {
			uiCanvas.gameObject.SetActive (true);
		} else
		{
			uiCanvas.gameObject.SetActive (false);
		}

		if (isBuilding) {
			SetTime ("Build");
			if (timeLeft <= 0) {
				upgradeTimeText.gameObject.SetActive (false);
				GameManagerScript.Instance.SetWorker (1);
				isBuilding= false;
				//Kalau Res Col
				if (this.GetComponent<ResourceCollectorScript> ())
					this.GetComponent<ResourceCollectorScript> ().ProduceResources ();
				//Kalau Camp
				if (this.GetComponent<CampScript> ())
					TroopsManager.Instance.addCamp(this.gameObject);
			}

		}

		if (isUpgrading) {
			uiCanvas.gameObject.SetActive (false );
			SetTime ("Upgrade");
			if (timeLeft <= 0) {
				level++;
				upgradeTimeText.gameObject.SetActive (false);
				GameManagerScript.Instance.SetWorker (1);
				isUpgrading = false;
				print ("upgraded to "+level);


				/////
			}
		}
	}

	void OnMouseDown () {
		Cursor.Instance.selectedObject = this.gameObject;
		print (Cursor.Instance.selectedObject.name);
	}

	public void Upgrade () {
		if (!isUpgrading && GameManagerScript.Instance.GetWorker () > 0) {
			timeLeft = upgradeTime;
			GameManagerScript.Instance.SetWorker (-1);
			isUpgrading = true;
		} else
			print ("boo");



//		isSelected = false;
//		upgradeButton.gameObject.SetActive (false);
	}

	public void Build () {
		timeLeft = buildTime;
		isBuilding = true;
	}

	void SetTime (string status) {
		minutes = Mathf.Floor (timeLeft/60).ToString("00");
		seconds = (timeLeft % 60).ToString ("00");
		upgradeTimeText.gameObject.SetActive (true);
		upgradeTimeText.text = status+" \n "+minutes+" min "+seconds+" sec";
		timeLeft -= Time.deltaTime;
	}

    public delegate void DestroyEvent(GameObject g);
    public event DestroyEvent destroyEvt;

	public void OnDestroy()
	{
		// trigger for entity destroy
		if (destroyEvt != null) destroyEvt(gameObject);
	}

}

