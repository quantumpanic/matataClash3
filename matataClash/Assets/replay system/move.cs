using UnityEngine;
using System.Collections.Generic;

public class move : MonoBehaviour {

	CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	float speed = 5;
	public bool isRecording;
	public List<Vector3> vectorList = new List<Vector3>();
	
	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
		isRecording = true;
	}
	
	void Update()
	{
		if (isRecording)
		{
            moveDirection = transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            controller.Move(moveDirection * Time.deltaTime);
			
			RecordForce(moveDirection);
		}
		else
		{
			moveDirection = vectorList[frameIndex];
            controller.Move(moveDirection * Time.deltaTime);
			frameIndex++;
			if (frameIndex >= vectorList.Count)
			{
				BeginSimulation();
			}
		}
	}
	
	public void BeginRecording()
	{
		isRecording = true;
		vectorList.Clear();
		transform.position = Vector3.zero;
	}
	
	void RecordForce(Vector3 force)
	{
		vectorList.Add(force);
	}
	
	public int frameIndex;
	
	public void BeginSimulation()
	{
		isRecording = false;
		frameIndex = 0;
		transform.position = Vector3.zero;
	}
}
