﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CampScript : MonoBehaviour {

	// Use this for initialization
	//public List<int> troops = new List<int>();
	int maxTroops = 5;
	int availableSlot;


	//Tar buang
	public Text tex;
	string a;

	void Start () {
		availableSlot = maxTroops;
		
	}
	//Tar buang
	void Update(){
		a = TroopsManager.Instance.troops.Count.ToString();
		tex.text = a;
	}
	
	public void addTroops (){
		//troops.Add(a);
		availableSlot--;
	}

	public bool isCampFull () {
		/*
		if(troops.Count < maxTroops) {
			print(troops.Count);
			return false;
		}
		else {
			print(troops.Count);
			return true;
		}
		*/
		if (availableSlot > 0) {
			return false;
		} else {
			return true;
		}
	}
	

	public int getAvailableSlot () {
		return availableSlot;
	}
}
