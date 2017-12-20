using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour {
	// This class is responsible for allowing background objects to move in various directions 
	// to give a fluid experience. 

	static float THRESHOLD = 10; // This threshold is needed to ensure that the background does not go too far in one direction. 

	// axis variables for the background
	float initX;
	float initY;
	float initZ;
	Rigidbody rb; // the Rigidbody object feature in Unity is what moves the object. Needs a variable to use it. 

	// Use this for initialization
	void Start () {
		rb = transform.GetComponent<Rigidbody>();
		initX = transform.position.x;
		initY = transform.position.y;
		initZ = transform.position.z;
	}

	// Update is called once per frame
	void Update () {
		int randTypeForce = Random.Range (0, 3); // Picks a random number between 0 and 3 to determine the direction to go to. 
		int force = Random.Range (5,10); // Picks a random force between 30 and 50. 

		if (randTypeForce == 0) {
			rb.AddForce (0, force, 0); // move up 
		}
		if (randTypeForce == 1) {
			rb.AddForce (force, 0, 0); // move right 
		}
		if (randTypeForce == 2) {
			rb.AddForce (0, -force, 0); // move down 
		}
		if (randTypeForce == 3) {
			rb.AddForce (-force, 0, 0); // move left 
		}

		// The two methods below ensure that the background does not go beyond the threshold. 

		if (transform.position.x > initX + THRESHOLD) {
			rb.AddForce (-force, 0, 0);
		} else {
			rb.AddForce (force, 0, 0);
		}

		if (transform.position.y > initY + THRESHOLD) {
			rb.AddForce (0, -force, 0);
		} else {
			rb.AddForce (0, force, 0);
		}
	}
}
