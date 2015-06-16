using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode()] 
public class LookAt : MonoBehaviour {

	public Transform target;
	// Use this for initialization
	void Start () {
		gameObject.transform.LookAt (target.position);
	}
	

}
