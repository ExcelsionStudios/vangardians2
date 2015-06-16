using UnityEngine;
using System.Collections;
//Script by Esai Solorio
//June 16, 2015


//Simple Script that attaches an enemy as a child of the hook
public class HookEnemy : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {
			col.gameObject.transform.parent = gameObject.transform;
		}
	}
}
