using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainScript : MonoBehaviour {

	//public GameObject trainUIPrefab;
	Transform trainUI;
    int footmanCost = 10;
    public GameObject camp;

    void Start(){
      //  Train();
    }

    public void Train() {
		//trainUI = (GameObject)Instantiate(trainUIPrefab, Vector3.zero, Quaternion.identity);
        trainUI = this.transform.GetChild(0).GetChild(2);
        trainUI.gameObject.SetActive(true);

        Button trainFootmanButton = trainUI.transform.GetChild(0).GetComponent<Button>();
        trainFootmanButton.onClick.AddListener(() => TrainFootman());
	}

    void TrainFootman(){
        if (GameManagerScript.Instance.GetMana() >= footmanCost){
            if (TroopsManager.Instance.isAllCampFull()){
                print("camp is full");
            } else {
                print("train 1 footman");
                TroopsManager.Instance.addTroops(1);
                GameManagerScript.Instance.SetMana(-footmanCost);
            }
        }else{
            print("not enough mana");
        }
       
    }
/*
    void TrainFootman(){
        if (GameManagerScript.Instance.GetMana() >= footmanCost){
            if (camp.GetComponent<CampScript>().isCampFull()){
                print("camp is full");
            } else {
                print("train 1 footman");
                camp.GetComponent<CampScript>().addTroops(1);
                GameManagerScript.Instance.SetMana(-footmanCost);
            }
        }else{
            print("not enough mana");
        }
       
    }
*/
}
