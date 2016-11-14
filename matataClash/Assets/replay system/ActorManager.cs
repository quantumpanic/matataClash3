using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorManager : MonoBehaviour {
	
	public static ActorManager Instance;
	
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public List<StageActor> actors = new List<StageActor>();
	
	public StageActor NewActor()
	{
		StageActor sa = StageActor.CreateComponent(Vector3.one, Quaternion.identity, "test");
		actors.Add(sa);
		
		return sa;
	}
	
	void MoveAllActors(Frame target, bool interpolate = true)
	{
		// moves all active actors forward from
		// the current frame to the target frame
		// with the option to use interpolation
	}
}
