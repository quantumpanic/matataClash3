using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceDisplayScript : MonoBehaviour {

	public Text goldText;
	public Text manaText;
	public Text workerText;



	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {		
		//goldText.text = "GOLD  : " + GameManagerScript.Instance.GetGold ().ToString ();
		//manaText.text = "MANA  : " + GameManagerScript.Instance.GetMana ().ToString ();
		//workerText.text = "WORKER : " + GameManagerScript.Instance.GetWorker ().ToString ();
		goldText.text = GameManagerScript.Instance.GetGold ().ToString ();
		manaText.text = GameManagerScript.Instance.GetMana ().ToString ();
		workerText.text = GameManagerScript.Instance.GetWorker ().ToString ();

	}
}
