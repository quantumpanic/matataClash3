using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainScript : MonoBehaviour {
	Transform trainUI;
    int footmanCost = 10;
    int querychanCost = 20;
    public GameObject camp;

    void Awake(){
        trainUI = this.transform.GetChild(0).GetChild(2);
        Button closeButton = trainUI.transform.GetChild(0).GetComponent<Button>();
        closeButton.onClick.AddListener(() => CloseTrainPanel());
        Button trainFootmanButton = trainUI.transform.GetChild(1).GetComponent<Button>();
        trainFootmanButton.onClick.AddListener(() => TrainFootman());
        Button trainQuerychanButton = trainUI.transform.GetChild(2).GetComponent<Button>();
        trainQuerychanButton.onClick.AddListener(() => TrainQueryChan());
    }

    public void Train() {        
        trainUI.gameObject.SetActive(true);        
	}

    void TrainFootman(){
        if (GameManagerScript.Instance.GetMana() >= footmanCost){
            if (TroopsManager.Instance.isAllCampFull()){
                TextAnimManager.Instance.WarningCampFull();
            } else {
                TextAnimManager.Instance.CustomWarning("1 Footman Trained", Color.blue);
                TroopsManager.Instance.addTroops(1);
                GameManagerScript.Instance.SetMana(-footmanCost);
            }
        }else{
            print("not enough mana");
            TextAnimManager.Instance.WarningNoMana();
        }
       
    }

    void TrainQueryChan(){
        if (GameManagerScript.Instance.GetMana() >= querychanCost){
            if (TroopsManager.Instance.isAllCampFull()){
                print("camp is full");
                TextAnimManager.Instance.WarningCampFull();
            } else {
                print("train 1 querychan");
                TroopsManager.Instance.addTroops(2);
                GameManagerScript.Instance.SetMana(-querychanCost);
            }
        }else{
            print("not enough mana");
            TextAnimManager.Instance.WarningNoMana();
        }
       
    }

    void CloseTrainPanel() {
        trainUI.gameObject.SetActive(false);
    }
}
