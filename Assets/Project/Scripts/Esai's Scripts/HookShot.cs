using UnityEngine;
using System.Collections;

public class HookShot : MonoBehaviour {


	//Script by Esai Solorio
	//June 15, 2015



	// Use this for initialization
	public Vector2 direction;
	public Vector2 initialMousePosition;
	public Vector2 lastMousePosition;
	public GameObject hookHead;
	public float hookSpeed;
	public float shootingDistance;
	public Transform hookStartPosition;
	public GameObject hookedObject;
	bool mouseClicked = false;
	bool shootHook = false;
	bool enemyHooked = false;
	bool throwObject = false;
	Vector2 throwDirection;
	bool slam = false;
	float slamDistance;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		/******Touch Stuff********/
		if (Input.touchCount > 0) { //Detect if device has been touched

			
			if (Input.GetTouch (0).phase == TouchPhase.Ended && mouseClicked) { //If touch has ended stop updating direction
				mouseClicked = false;
				shootHook = true;
				shootingDistance = Vector3.Distance (Camera.main.ScreenToWorldPoint(initialMousePosition), Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));

			}
			if (Input.GetTouch (0).phase == TouchPhase.Moved && mouseClicked) { //If touch is continuing update direction 
				lastMousePosition = Input.GetTouch(0).position;
				getDirection();
			}
		}
		/******Touch Stuff********/



		/*****Mouse Stuff********/

	
		if (Input.GetKeyUp (KeyCode.Mouse0) && mouseClicked) {  //If mouse drag ended stop updating direction
			mouseClicked = false;
			shootHook = true;
			shootingDistance = Vector3.Distance (Camera.main.ScreenToWorldPoint(initialMousePosition), Camera.main.ScreenToWorldPoint(lastMousePosition));

		}
		if (mouseClicked) { 
			lastMousePosition = Input.mousePosition;
			getDirection ();
		}
		/******Mouse Stuff******/




		if (shootHook && Vector3.Distance (hookHead.transform.position, gameObject.transform.position) <= shootingDistance) {  // Shoots the hook
			hookHead.transform.position += hookHead.gameObject.transform.forward * hookSpeed;
		} else if(!enemyHooked){
			hookHead.transform.position = Vector3.MoveTowards(hookHead.transform.position,hookStartPosition.position, hookSpeed);
			shootingDistance = 0;
		}

		if (Input.GetKey (KeyCode.Mouse0) && enemyHooked) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//Debug.DrawRay (ray.origin, ray.direction * 50, Color.red);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 50)) {
				if(hit.collider.gameObject == hookedObject){
					throwObject = true;
					slamDistance = Vector3.Distance(hookedObject.transform.position, gameObject.transform.position);
				}
			}

		}
		if (throwObject) {
			//get direction of throw
			throwDirection = -(Camera.main.WorldToScreenPoint(hookedObject.transform.position) - Input.mousePosition)/(Camera.main.WorldToScreenPoint(hookedObject.transform.position) - Input.mousePosition).magnitude;
			Ray throwRay = new Ray(hookedObject.transform.position, new Vector3(throwDirection.x, 0 , throwDirection.y));
			RaycastHit hit;
			Debug.DrawRay (throwRay.origin, throwRay.direction * 50, Color.red);
			if(Physics.Raycast(throwRay, out hit, 50)){
				if(hit.collider.gameObject.tag == "Player"){
					slam = true;
				}
			}
		}

		if (slam) {
			hookedObject.transform.position = throwDirection * hookSpeed;
		}


		//Updates the direction of the marker
		gameObject.transform.rotation = Quaternion.LookRotation (new Vector3(direction.x, 0 , direction.y));
	}

	void getDirection(){
		direction = (initialMousePosition - lastMousePosition)/(initialMousePosition - lastMousePosition).magnitude;
	}

	void OnMouseDown(){
		initialMousePosition = Input.mousePosition;
		mouseClicked = true;
	}

	public void HookEnemy(GameObject objectHooked){
		enemyHooked = true;
		hookedObject = objectHooked;
	}
}
