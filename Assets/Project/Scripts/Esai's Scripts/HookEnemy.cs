using UnityEngine;
using System.Collections;
//Script by Esai Solorio
//June 16, 2015


//Simple Script that attaches an enemy as a child of the hook
public class HookEnemy : MonoBehaviour {
	public HookShot hookShot;
	public GameObject haloPrefab;
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy" && !hookShot.enemyHooked) {
			col.gameObject.transform.parent = gameObject.transform;
			col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			GameObject myHalo = (GameObject)Instantiate(haloPrefab, col.gameObject.transform.position, haloPrefab.transform.rotation);
			myHalo.transform.parent = col.gameObject.transform;
			hookShot.HookEnemy(col.gameObject);
		}
	}
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Enemy") {
			hookShot.hookedObject.GetComponent<Rigidbody>().isKinematic = false;
			hookShot.hookedObject = null;
			
			foreach (Transform child in col.gameObject.transform) {
				
				Destroy(child.gameObject);
			}
		}
	}
}
