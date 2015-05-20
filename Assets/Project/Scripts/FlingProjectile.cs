﻿using UnityEngine;
using System.Collections;

public class FlingProjectile : MonoBehaviour {
	public GameObject projectilePrefab;
	public float upwardForce;
	public float sideForce;
	public Vector2 lastMousePosition;
	public Vector2 direction;
	public float maxForce;
	public bool usePrediction;

	public Material transparentMaterial;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			lastMousePosition = Input.mousePosition;
		}
		if (usePrediction && Input.GetKey (KeyCode.Mouse0)) {
			direction = new Vector2 (Mathf.Clamp(Input.mousePosition.x - lastMousePosition.x, -maxForce, maxForce)/maxForce,Mathf.Clamp(Input.mousePosition.y - lastMousePosition.y, -maxForce, maxForce)/maxForce );
			GameObject newProjectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, projectilePrefab.transform.rotation);
			newProjectile.GetComponent<Rigidbody>().AddForce(Vector3.up * upwardForce);
			newProjectile.GetComponent<Rigidbody>().AddForce(new Vector3(-direction.x, 0, -direction.y) * sideForce);
			newProjectile.GetComponent<Renderer>().material = transparentMaterial;
			Destroy(newProjectile.GetComponent<Collider>());
		}
		if (Input.GetKeyUp(KeyCode.Mouse0)) {
			direction = new Vector2 (Mathf.Clamp(Input.mousePosition.x - lastMousePosition.x, -maxForce, maxForce)/maxForce,Mathf.Clamp(Input.mousePosition.y - lastMousePosition.y, -maxForce, maxForce)/maxForce );
			GameObject newProjectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, projectilePrefab.transform.rotation);
			newProjectile.GetComponent<Rigidbody>().AddForce(Vector3.up * upwardForce);
			newProjectile.GetComponent<Rigidbody>().AddForce(new Vector3(-direction.x, 0, -direction.y) * sideForce);
		}
	}


}
