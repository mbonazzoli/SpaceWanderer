using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour {

	public GameObject planet;
	GameObject planet_clone;

    Texture2D[] solids;
	Texture2D[] overlays;

	public Sprite[] solidSprites;
	public Sprite[] overlaySprites;

	Transform princeTransform;

	Camera camera;

	void Start () {
		camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>() ;
		princeTransform = GameObject.FindGameObjectWithTag ("Prince").transform;

		loadTextures ();
		solidSprites = createSpriteList (solids);
		overlaySprites = createSpriteList (overlays);

		for (int i = 0; i <30; i++) {
			createNewPlanet ();
		}
	}

	//Loads the planet layers from the resource folder and stores the textures in an array
	void loadTextures(){
		solids = Resources.LoadAll<Texture2D> ("PlanetTextures/Solids");
		overlays = Resources.LoadAll<Texture2D> ("PlanetTextures/Overlays");
	}

	//Creates sprites out of each layer and stores them in an array
	//This allows for faster processing and no lag while planets are changed
	Sprite[] createSpriteList(Texture2D[] textures){
		Sprite[] sprites = new Sprite[textures.Length];

		for(int i = 0;i < sprites.Length;i++) {
			Texture2D texture = textures [i];
			sprites[i] = Sprite.Create (texture, new Rect (0.0f, 0.0f, texture.width, texture.height), new Vector2 (0.5f, 0.5f), 20.0f) as Sprite;
		}
		return sprites;
	}

	//Instantiates a planet clone using created planet prefab and assigns sprite layers as children to the game object
	void createNewPlanet(){
		float randZ = Random.Range (200, 500);

		float frustumH = calculateFrustumHeight(randZ);
		float frustumW = calculateFrustumWidth (randZ, frustumH);

		int xRange = (int)frustumW / 2;
		int yRange = (int)frustumH / 2;

		float randX = Random.Range (-xRange, xRange);
		float randY = Random.Range (-yRange, yRange);

		planet_clone = (GameObject)Instantiate (planet, new Vector3 (randX, randY , randZ), transform.rotation) as GameObject;
		planet_clone.tag = "Planet";

		composePlanet (planet_clone);
		setPlanetRenderOrder (planet_clone);
	}

	//Calculates the camera frame height depending on the z-position of the planet object so the planet will be generated in the bounds of the camera frame
	public int calculateHeightRange(float zPos){
		return (int)(calculateFrustumHeight (zPos) / 2);
	}
		
	//Calculates the camera frame width depending on the z-position of the planet object so the planet will be generated in the bounds of the camera frame
	public int calculateWidthRange(float zPos){
		float fHeight = calculateFrustumHeight (zPos);
		return (int)(calculateFrustumWidth (zPos, fHeight) / 2);
	}

	//Calculates the height of the frustum plane at a given z-position
	float calculateFrustumHeight(float distance){
		return 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
	}

	//Calculates the width of the frustum plane at a given z-position
	float calculateFrustumWidth(float distance, float frustumHeight){
		return frustumHeight * camera.aspect;
	}

	//Given a planet game object, any previous layers of the planet are destroyed and new layers replace them.
	public void composePlanet(GameObject planet){
		if (planet.transform.childCount > 0) {
			for (int i = planet.transform.childCount-1; i >= 0; i--) {
				Destroy (planet.transform.GetChild (i).gameObject);
			}
		}
		addLayerAsChild (planet, solidSprites);
		addLayerAsChild (planet, overlaySprites);
		addLayerAsChild (planet, overlaySprites);
		addLayerAsChild (planet, overlaySprites);
	}

	//Adds a planet layer as a sprite to the current parent planet game object
	void addLayerAsChild(GameObject parent, Sprite[] spriteList){
		GameObject child = new GameObject ();
		child.transform.SetParent (parent.transform, false);
		child.transform.localPosition = Vector3.zero;

		SpriteRenderer childSR = child.AddComponent<SpriteRenderer> ();

		assignLayer (child, spriteList);
	}

	public void assignLayer(GameObject child, Sprite[] sprites){
		child.GetComponent<SpriteRenderer> ().sprite = sprites [Random.Range (0, sprites.Length)];
//		initializeTransparency (child.GetComponent<SpriteRenderer> ());
		addSpinToLayer (child);
	}

	//adds spin to each planet layer individually to give cool cloud effect
	void addSpinToLayer(GameObject child){
		if (child.GetComponent<PlanetLayerSpin> () == null) {
			PlanetLayerSpin spin = child.AddComponent<PlanetLayerSpin> ();
			spin.speed = Random.Range (-5, 5);
			spin.axis = Vector3.forward;
		}
	}

	//sets the overal planet object's render order so the closer planets get rendered in front of further planets
	public void setPlanetRenderOrder(GameObject planet){
		SpriteRenderer parentRenderer = planet.GetComponent<SpriteRenderer> ();

		int parentRenderOrder = -((int)planet.transform.position.z%2000)*5;
		parentRenderer.sortingOrder = parentRenderOrder;

		for (int i = 0; i < planet.transform.childCount; i++) {
			setChildRenderOrder (parentRenderOrder, planet.transform.GetChild (i).GetComponent<SpriteRenderer> (), i);
		}
	}

	//Sets the render order for each layer so they appear correctly
	public void setChildRenderOrder(int parentRenderOrder, SpriteRenderer childSR, int childNum){
		childSR.sortingOrder = parentRenderOrder + childNum;
	}
}
