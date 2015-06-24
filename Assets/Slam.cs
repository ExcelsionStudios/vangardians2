using UnityEngine;
using System.Collections;

public class Slam : MonoBehaviour {

	//Script created by Esai Solorio on Jun 24, 2015



	Quaternion targetRotation;
	public float smooth = 3;
	public bool start;
	public float epsilon = .01f;
	void Start(){
		targetRotation = transform.rotation;
	}
	void Update(){
		if (start) {
			targetRotation = Quaternion.AngleAxis(180, gameObject.transform.right) * transform.rotation;
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
