using UnityEngine;
using System.Collections;
// Written by Esai Solorio
// May 21, 2015
public class ScatterShot : MonoBehaviour {

	void Update () {
		if (gameObject.GetComponent<Rigidbody> ().velocity.y < 0) { //When the projectile reaches its apex
			
			
			
			
			foreach(Transform spawnPoint in gameObject.transform.Find("SpawnPoints").GetComponentsInChildren<Transform>()){ 	//gets a collection of points from the projectile
				GameObject newProjectile = (GameObject)Instantiate(gameObject, spawnPoint.position, gameObject.transform.rotation);		//create a new projectile
				newProjectile.transform.localScale *= .5f; //shrink the new projectile
				Destroy (newProjectile.GetComponent<ScatterShot>()); //get rid of the shattershot script (otherwise it will continuously multiply)
				newProjectile.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity; //give the new projectiles the same velocity as the original
			}
			
			
			
			Destroy (gameObject);
		}
	}
}
