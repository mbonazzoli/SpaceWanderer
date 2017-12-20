using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePlanetMove : MonoBehaviour {
	void Update () {
		float step = Time.deltaTime * 20;
		transform.position = Vector3.MoveTowards (transform.position, new Vector3(transform.position.x,transform.position.y,transform.position.z + 5), step);
	}
}
