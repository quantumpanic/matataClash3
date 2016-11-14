using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ReelManager : MonoBehaviour {

	public static ReelManager Instance;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	// Use this for initialization
	void Start () {
		ActorManager.Instance.NewActor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// some functions to:
	// store the movements of actors
	// count the frames
	// 
	
	public void BeginRecording()
	{
		Recorder.Instance.Record();
	}
	
	public void StopRecording()
	{
		Recorder.Instance.Stop();
	}
	
	void WriteToMemory()
	{
		MemoryStream ms = new MemoryStream();    
		FileStream file = new FileStream("file.bin", FileMode.Create, FileAccess.Write);
		ms.WriteTo(file);
		file.Close();
		ms.Close();
	}
	
	void ReadFromMemory()
	{
		
	}
	
	void CleanMemory()
	{
		
	}
}
