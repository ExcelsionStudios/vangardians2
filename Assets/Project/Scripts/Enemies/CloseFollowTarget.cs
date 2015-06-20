using UnityEngine;
using System.Collections;

public class CloseFollowTarget : MonoBehaviour {

	// Use this for initialization
	public Transform target;
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = target.transform.position;
	}
}
