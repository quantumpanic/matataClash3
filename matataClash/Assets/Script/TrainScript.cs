using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainScript : MonoBehaviour {

	//public GameObject trainUIPrefab;
	Transform trainUI;
    int footmanCost = 10;
    public GameObject camp;

    void Awake(){
        trainUI = this.transform.GetChild(0).GetChild(2);
        Button closeButton = trainUI.transform.GetChild(0).GetComponent<Button>();
        closeButton.onClick.AddListener(() => CloseTrainPanel());
        Button trainFootmanButton = trainUI.transform.GetChild(1).GetComponent<Button>();
        trainFootmanButton.onClick.AddListener(() => TrainFootman());
    }

    public void Train() {        
        trainUI.gameObject.SetActive(true);        
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

    void CloseTrainPanel() {
        trainUI.gameObject.SetActive(false);
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
