using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour {

	Quaternion targetRotation;
	public float smooth = 3;
	public bool start = true;
	public float epsilon = 2;
	void Start(){
		targetRotation = transform.rotation;
	}
	void Update(){
		if (start) {
			targetRotation = Quaternion.AngleAxis(90, gameObject.transform.up) * transform.rotation;
			start = false;
		}
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, smooth * Time.deltaTime);
		
		if (Quaternion.Angle(targetRotation, transform.rotation) <= epsilon) {
			this.enabled = false;
		}
		
	}
	void OnDisable(){
		start = true;
		GameObject.Find ("Player").GetComponent<HookShot> ().hookedObject.transform.parent = null;
		GameObject.Find ("Player").GetComponent<HookShot> ().enemyHooked = false;
		GameObject.Find ("Player").GetComponent<HookShot> ().throwObject = false;
	}
}
