using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingClouds: MonoBehaviour {
	// This method is responsible for moving the clouds in various directions. 

	static float THRESHOLD = 2;

	float initX;
	float initY;
	float initZ;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = transform.GetComponent<Rigidbody>();
		initX = transform.position.x;
		initY = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		int randTypeForce = Random.Range (0, 3);
		int force = Random.Range(5, 7);  

		if (randTypeForce == 0) { 
			rb.AddForce (0, force, 0);
		}
		if (randTypeForce == 1) {
			rb.AddForce (force, 0, 0);
		}
		if (randTypeForce == 2) {
			rb.AddForce (0, -force, 0);
		}
		if (randTypeForce == 3) {
			rb.AddForce (-force, 0, 0);
		}


		if (transform.position.x > initX + THRESHOLD) {
			rb.AddForce (-force, 0, 0);
		} else if (transform.position.x < initX - THRESHOLD) {
			rb.AddForce (force, 0, 0);
		}

		if (transform.position.y > initY + THRESHOLD) {
			rb.AddForce (0, -force, 0);
		} else if (transform.position.y < initY - THRESHOLD) {
			rb.AddForce (0, force, 0);
		}
	}
}
