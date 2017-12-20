using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRegeneration : MonoBehaviour {

	bool hasEverBeenVisible;

	SpriteRenderer sr;

	PlanetGenerator generatorScript;

	GameObject prince;
	Transform princeTransform;
	float princeOffset;

	void Start () {
		hasEverBeenVisible = false;

		sr = gameObject.GetComponent<SpriteRenderer> ();

		prince = GameObject.FindGameObjectWithTag ("Prince");
		princeTransform = prince.GetComponent<Transform> ();
		princeOffset = 0;

		generatorScript = GameObject.FindGameObjectWithTag ("Planet").GetComponent<PlanetGenerator> ();

	}
		
	void Update(){
		princeOffset = princeTransform.position.z;
		if (hasEverBeenVisible && !anyChildrenAreVisible()) {
			relocate ();
			for (int i = 0; i < transform.childCount; i++) {
				if (i == 0) {
					generatorScript.assignLayer (transform.GetChild (i).gameObject, generatorScript.solidSprites);
				} else {
					generatorScript.assignLayer (transform.GetChild (i).gameObject, generatorScript.overlaySprites);
				}
			}
		}
		adjustRenderOrderForPrincePos ();
	}
		
	// Relocate and change layers of planet object to create a "new" planet
	void relocate(){
		float zRand = Random.Range (600, 650);
		float zPos = princeOffset + zRand;

		float xRange = generatorScript.calculateWidthRange (zRand);
		float xRand = Random.Range (-xRange, xRange);
		float xPos = xRand + princeTransform.position.x;

		float yRange = generatorScript.calculateHeightRange (zRand);
		float yRand = Random.Range (-yRange, yRange);
		float yPos = yRand + princeTransform.position.y;

		transform.position = new Vector3 (xPos, yPos, zPos);
		generatorScript.setPlanetRenderOrder (gameObject);
	}

	//Checks if any of the actual planet layers are visible
	//used for relocate conditional
	bool anyChildrenAreVisible() {
		foreach(Renderer renderer in sr.GetComponentsInChildren<Renderer>()) {
			if(renderer.isVisible)
				return true;
		}
		return false;
	}

	//Build in method that runs once the planet has been rendered in the camera frame
	void OnBecameVisible() {
		hasEverBeenVisible = true;
	}

	//Adjusts the render order of the planets so they are in the correct order when the prince is flying
	void adjustRenderOrderForPrincePos(){
		GetComponent<SpriteRenderer> ().sortingOrder = -((int)(transform.position.z - princeTransform.position.z)%2000)*5;;
		PlanetGenerator generatorScript = GameObject.FindGameObjectWithTag ("Planet").GetComponent<PlanetGenerator> ();

		for (int i = 0; i < transform.childCount; i++) {
			generatorScript.setChildRenderOrder (GetComponent<SpriteRenderer> ().sortingOrder, transform.GetChild (i).GetComponent<SpriteRenderer> (), i);
		}
	}
}
