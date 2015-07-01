using UnityEngine;
using System.Collections;

//Script by Esai Solorio
//June 15, 2015

public class HookShot : MonoBehaviour 
{
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
	public bool enemyHooked = false;
	public bool throwObject = false;
	Vector2 throwDirection;
	bool slam = false;
	float slamDistance;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
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
			gameObject.transform.rotation = Quaternion.LookRotation (new Vector3(direction.x, 0 , direction.y));
		}
		/******Mouse Stuff******/
		
		
		
		// Shoots the hook
		if (shootHook && Vector3.Distance (hookHead.transform.position, gameObject.transform.position) <= shootingDistance) 
		{  
			hookHead.transform.position += hookHead.gameObject.transform.forward * hookSpeed;
		} 
		else if(!enemyHooked)
		{
			hookHead.transform.position = Vector3.MoveTowards(hookHead.transform.position,hookStartPosition.position, hookSpeed);
			shootingDistance = 0;
		}

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			if(Input.GetTouch(0).phase == TouchPhase.Ended && enemyHooked)
			{
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
				//Debug.DrawRay (ray.origin, ray.direction * 50, Color.red);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 50)) 
				{
					if(hit.collider.gameObject == hookedObject)
					{
						throwObject = true;
					}
				}
			}
			
			if (throwObject) 
			{
				//get direction of throw
				if(Input.GetTouch(0).phase == TouchPhase.Ended)
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
					Plane plane = new Plane(Vector3.up, transform.position);
					float distance = 0;
					if (plane.Raycast(ray, out distance))
					{ // if plane hit...
						Vector3 pos = ray.GetPoint(distance); // get the point
						// pos has the position in the plane you've touched
					}
						
					Vector3 touchPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, distance);
					
					throwDirection = -(Camera.main.WorldToScreenPoint(hookedObject.transform.position) - touchPos)/(Camera.main.WorldToScreenPoint(hookedObject.transform.position) - touchPos).magnitude;
					
					Ray throwRay = new Ray(hookedObject.transform.position, new Vector3(throwDirection.x, 0 , throwDirection.y));
					
					Debug.DrawRay (throwRay.origin, throwRay.direction * 50, Color.red);
					
					RaycastHit hit;
					
					if(Physics.Raycast(throwRay, out hit))
					{
						//Debug.Log(hit.collider.name);
						
						if(hit.collider.tag == "Player")
						{
							//Debug.Log("fuckkk yeah"); // lol...
							gameObject.GetComponent<Slam>().enabled = true;
						}
					} 
					else
					{
						Vector3 relativePoint = gameObject.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));

						if(relativePoint.x > 0)
						{
							gameObject.GetComponent<Swing>().swingRight();
						}
						else if(relativePoint.x < 0)
						{
							gameObject.GetComponent<Swing>().swingLeft();
						}
						gameObject.GetComponent<Swing>().enabled = true;
					}
				}
			}
			
		} 
		else 
		{
			if (Input.GetKey (KeyCode.Mouse0) && enemyHooked) 
			{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				//Debug.DrawRay (ray.origin, ray.direction * 50, Color.red);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 50)) 
				{
					if(hit.collider.gameObject == hookedObject)
					{
						throwObject = true;
					}
				}
				
			}
			
			if (throwObject) 
			{
				//get direction of throw
				if(Input.GetKeyUp(KeyCode.Mouse0)){
					throwDirection = -(Camera.main.WorldToScreenPoint(hookedObject.transform.position) - Input.mousePosition)/(Camera.main.WorldToScreenPoint(hookedObject.transform.position) - Input.mousePosition).magnitude;
					Ray throwRay = new Ray(hookedObject.transform.position, new Vector3(throwDirection.x, 0 , throwDirection.y));
					
					Debug.DrawRay (throwRay.origin, throwRay.direction * 50, Color.red);
					
					RaycastHit hit;
					
					if(Physics.Raycast(throwRay, out hit))
					{
						//Debug.Log(hit.collider.name);
						
						if(hit.collider.tag == "Player")
						{
							//Debug.Log("fuckkk yeah");
							gameObject.GetComponent<Slam>().enabled = true;
						}
					} 
					else
					{
						Vector3 relativePoint = gameObject.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

						if(relativePoint.x > 0)
						{
							gameObject.GetComponent<Swing>().swingRight();
						}
						else if(relativePoint.x < 0)
						{
							gameObject.GetComponent<Swing>().swingLeft();
						}
						gameObject.GetComponent<Swing>().enabled = true;
					}
				}
			}
		}

		//Updates the direction of the marker
	
	}
	
	void getDirection()
	{
		direction = (initialMousePosition - lastMousePosition)/(initialMousePosition - lastMousePosition).magnitude;
	}
	
	void OnMouseDown()
	{
		initialMousePosition = Input.mousePosition;
		mouseClicked = true;
	}
	
	public void HookEnemy(GameObject objectHooked)
	{
		enemyHooked = true;
		hookedObject = objectHooked;
	}
}

