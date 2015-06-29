using UnityEngine;
using System.Collections;

public class Slam : MonoBehaviour {
	
	//Script created by Esai Solorio on Jun 24, 2015
	
	
	
	Quaternion targetRotation;
	public float smooth = 3;
	public bool start;
	public float epsilon = .01f;
	public GameObject explosionPrefab;
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
		
		Instantiate (explosionPrefab, GameObject.Find ("Player").GetComponent<HookShot> ().hookedObject.transform.position, explosionPrefab.transform.rotation);
		
		GameObject.Find ("Player").GetComponent<HookShot> ().hookedObject.GetComponent<Rigidbody> ().useGravity = true;
		//
		GameObject.Find ("Player").GetComponent<HookShot> ().hookedObject.transform.parent = null;
		GameObject.Find ("Player").GetComponent<HookShot> ().enemyHooked = false;
		GameObject.Find ("Player").GetComponent<HookShot> ().throwObject = false;
		
		
	}
	
}

