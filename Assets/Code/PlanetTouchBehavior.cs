using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Specialized;
using UnityEngine.Experimental.UIElements.StyleEnums;
//using UnityEditorInternal;

public class PlanetTouchBehavior : MonoBehaviour {
	GameObject target = null;
	string princeState = "standing";
	public Sprite jump;
	public Sprite down;
	public Sprite stand;
	Vector3 finalTargetCoord;
	Vector3 halfTargetCoord;


	void Start () {
	}

	void Update () {
		if (Input.touchCount == 1) {
			// The screen has been touched so store the touch

			Touch touch = Input.GetTouch (0); 
			if (touch.phase == TouchPhase.Ended) {
				

				var touchPosition = touch.position;
				print ("Touch Position: " + touchPosition);
				Camera c = Camera.main;
				print ("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);

				//FIND THE LOCAL TOUCH POSITION
				var screenTouch = c.WorldToScreenPoint (touchPosition);
				print ("Screen Touch: " + screenTouch);
				Vector3 worldTouch = c.ScreenToWorldPoint (new Vector3 (touchPosition.x, touchPosition.y, transform.position.z + 500));
				print ("World: " + worldTouch);
				target = findClosestPlanet (touchPosition);
				princeState = "up";
				print ("Prince Position: " + transform.position);
				transform.parent.tag = "Planet";
				Destroy (GetComponentInParent <HomePlanetMove>());
				transform.parent = null;
				finalTargetCoord = new Vector3 (target.transform.position.x, target.transform.position.y+25, target.transform.position.z);
				halfTargetCoord = new Vector3 ((target.transform.position.x + transform.position.x)/2, target.transform.position.y+25+70, (target.transform.position.z + transform.position.z)/2);
			}
		}

		if (target != null){

			float step = Time.deltaTime * 90;
			if (princeState.Equals ("up")) {
				GetComponent <SpriteRenderer>().sprite = jump;
				transform.position = Vector3.MoveTowards (transform.position, halfTargetCoord, step);
				if (transform.position == halfTargetCoord)
					princeState = "down";
			} 
			if (princeState.Equals("down")) {
				GetComponent <SpriteRenderer>().sprite = down;
				transform.position = Vector3.MoveTowards (transform.position, finalTargetCoord, step);
				if (transform.position == finalTargetCoord) {
					GetComponent <SpriteRenderer>().sprite = stand;
					target.tag = "HomePlanet";
					transform.parent = target.transform;
					princeState = "standing";
					target.AddComponent <HomePlanetMove>();
					target.GetComponent <HomePlanetMove>().enabled = true;
					target = null;
				}
			}
		}

	}

	private GameObject findClosestPlanet(Vector2 touch) 
	// https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html has the original code this method was based off from 
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Planet");
		print ("Game Objects: " + gos);
		GameObject closestPlanet = null;
		float distance = Mathf.Infinity;
		foreach (GameObject go in gos)
		{
			print ("Planet Position:" + go.transform.position + "| Screen Position: " + Camera.main.WorldToScreenPoint(go.transform.position));
			//planet position - touch position
			Vector2 diff = new Vector2(Math.Abs(Camera.main.WorldToScreenPoint(go.transform.position).x - touch.x), Math.Abs(Camera.main.WorldToScreenPoint(go.transform.position).y - touch.y));
			// curDistance = the radius^2 bewteen the touch and planet
			float curDistance = (float)(Math.Pow (diff.x, 2) + Math.Pow (diff.y, 2));

			// if the distance between touch and planet is not infinite, make the distance to be the touch_vs_planet_distance
			if (curDistance < distance)
			{
				closestPlanet = go;
				distance = curDistance;
			}
		}
		return closestPlanet;
	}
}