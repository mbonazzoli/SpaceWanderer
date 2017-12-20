using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownBackgroundBehavior : MonoBehaviour {
	//This gives the feeling of infinite background 

	public float backgroundSize;

	private Transform cameraTransform;
	private Transform[] layers;
	private float viewzone = 10;
	private int downIndex;
	private int upIndex;

	private void Start()
	{
		cameraTransform = Camera.main.transform;
		layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++){
			layers[i] = transform.GetChild(i);
		}
		downIndex = 0;
		upIndex = layers.Length -1;
	}

	private void ScrollDown()
	{
		int lastUp = upIndex;
		layers[upIndex].position = Vector3.forward*(layers[downIndex].position.y - backgroundSize);
		downIndex = upIndex;
		upIndex--;
		if(upIndex < 0) {
			upIndex = layers.Length-1;
		}
	}

	private void ScrollUp()
	{
		int lastDown = downIndex;
		layers[downIndex].position = Vector3.forward*(layers[upIndex].position.y + backgroundSize);
		upIndex = downIndex;
		downIndex++;
		if(downIndex == layers.Length) 
			downIndex = 0;
	}
		
	private void Update()
	{
		if (cameraTransform.position.y < (layers[downIndex].transform.position.y + viewzone))
		ScrollDown();

		if (cameraTransform.position.y > (layers[upIndex].transform.position.y - viewzone))
		ScrollUp();
	}
}
