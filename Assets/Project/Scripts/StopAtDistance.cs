using UnityEngine;
using System.Collections;


// Created by Esai Solorio
// May 22, 2015

public class StopAtDistance : MonoBehaviour {
	public int distance;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (gameObject.transform.position, gameObject.GetComponent<MoveTo> ().target.position) <= distance) {
			gameObject.GetComponent<MoveTo> ().enabled = false;
		} else {
			gameObject.GetComponent<MoveTo> ().enabled = true;
		}
		//Debug.Log("" + Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<MoveTo> ().target.position));
	}
}
