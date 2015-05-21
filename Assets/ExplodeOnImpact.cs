using UnityEngine;
using System.Collections;

public class ExplodeOnImpact : MonoBehaviour {

	public GameObject explodePrefab;
	// Use this for initialization
	void OnCollisionEnter(Collision col){
		Instantiate (explodePrefab, gameObject.transform.position, explodePrefab.transform.rotation);
	}

}
