using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	public Transform target;
	// Use this for initialization
	void Start () {
		gameObject.transform.LookAt (target.position);
	}
	

}
