using UnityEngine;
using System.Collections;

public class GameManagerScript : ScriptableObject{

	private static GameManagerScript instance = null; 

	private int gold = 100;
	private int mana = 1000;
	private int worker = 2;

	protected GameManagerScript() {}

	public GameObject selectedBuilding;

	// Singleton pattern implementation
	public static GameManagerScript Instance {
		get {
			if (instance == null) {
//				instance = new GameManagerScript();
				instance = ScriptableObject.CreateInstance <GameManagerScript> ();
			}  
			return instance;
		}
	}
		
	public int GetGold () {
		return gold;
	}

	public void SetGold (int x) {
		gold += x;
		Debug.Log(" added"+ x);
	}

	public int GetMana () {
		return mana;
	}

	public void SetMana (int x) {
		mana += x;
	}

	public int GetWorker () {
		return worker;
	}

	public void SetWorker (int x) {
		worker += x;
	}

	
}
