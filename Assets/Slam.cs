using UnityEngine;
using System.Collections;

public class Slam : MonoBehaviour {

	Quaternion targetRotation;
	public float smooth = 3;
	public bool start;
	void Start(){
		targetRotation = transform.rotation;
	}
	void Update(){
		if (start) {
			targetRotation = Quaternion.AngleAxis(180, gameObject.transform.right) * transform.rotation;
			start = false;
		}
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, smooth * Time.deltaTime);


	}
	/*
	var targetRotation : Quaternion;
	var smooth : float = 3.0;
	
	function Start() {
		targetRotation = transform.rotation;
	}
	
	function Update() {
		if (Input.GetKeyDown(KeyCode.Keypad0)) {
			targetRotation = Quaternion.AngleAxis(180.0, transform.forward) * transform.rotation;
		}
		
		transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, smooth * Time.deltaTime);
	} */
}
