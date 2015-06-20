using UnityEngine;
using System.Collections;
// Written by Esai Solorio
// May 22, 2015
public class AttackOnStop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.GetComponent<MoveTo> ().enabled) {
			gameObject.GetComponentInChildren<Animator>().enabled = true;
		}
	}
}
