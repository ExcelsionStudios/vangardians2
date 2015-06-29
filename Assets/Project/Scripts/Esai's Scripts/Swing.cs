using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour {
	
	Quaternion targetRotation;
	public float smooth = 3;
	public bool start;
	public float epsilon = 2;
	public float swingAmmount = 180;
	
	Vector3 swingDirection;
	void Start(){
		targetRotation = transform.rotation;
	}
	void Update(){
		if (start) {
			targetRotation = Quaternion.AngleAxis(swingAmmount, swingDirection) * transform.rotation;
			start = false;
		}
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, smooth * Time.deltaTime);
		
		if (Quaternion.Angle(targetRotation, transform.rotation) <= epsilon) {
			this.enabled = false;
		}
		
	}
	public void swingLeft(){
		swingDirection = -gameObject.transform.up;
		Debug.Log ("Swinging left");
	}
	public void swingRight(){
		swingDirection = gameObject.transform.up;
		Debug.Log ("Swinging right");
	}
	void OnDisable(){
		start = true;
		
		//GameObject.Find ("Player").GetComponent<HookShot> ().hookedObject.GetComponent<Rigidbody> ().isKinematic = false;
		
		GameObject.Find ("Player").GetComponent<HookShot> ().hookedObject.GetComponent<Rigidbody> ().useGravity = true;
		GameObject.Find ("Player").GetComponent<HookShot> ().hookedObject.transform.parent = null;
		GameObject.Find ("Player").GetComponent<HookShot> ().enemyHooked = false;
		GameObject.Find ("Player").GetComponent<HookShot> ().throwObject = false;
		
		
	}
}
