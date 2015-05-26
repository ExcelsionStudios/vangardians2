using UnityEngine;
using System.Collections;

public class SpreadFireOnImpact : MonoBehaviour {

	// Use this for initialization
	public GameObject FireSpreadPrefab;

	void OnCollisionEnter(Collision col){
		Instantiate (FireSpreadPrefab, gameObject.transform.position, FireSpreadPrefab.transform.rotation);
	}
}
