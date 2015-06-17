﻿using UnityEngine;
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
	bool mouseClicked = false;
	bool shootHook = false;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		/******Touch Stuff********/
		if (Input.touchCount > 0) { //Detect if device has been touched

			
			if (Input.GetTouch (0).phase == TouchPhase.Ended && mouseClicked) { //If touch has ended stop updating direction
				mouseClicked = false;
				shootHook = true;
			}
			if (Input.GetTouch (0).phase == TouchPhase.Moved) { //If touch is continuing update direction 
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
		} else {
			hookHead.transform.position = Vector3.MoveTowards(hookHead.transform.position,hookStartPosition.position, hookSpeed);
			shootingDistance = 0;
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
}