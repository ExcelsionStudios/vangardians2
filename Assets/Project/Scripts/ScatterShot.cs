using UnityEngine;
using System.Collections;

public class ScatterShot : MonoBehaviour {

	void Update () {
		if (gameObject.GetComponent<Rigidbody> ().velocity.y < 0) {
			
			
			
			
			foreach(Transform spawnPoint in gameObject.transform.Find("SpawnPoints").GetComponentsInChildren<Transform>()){
				GameObject newProjectile = (GameObject)Instantiate(gameObject, spawnPoint.position, gameObject.transform.rotation);
				newProjectile.transform.localScale *= .5f;
				Destroy (newProjectile.GetComponent<ScatterShot>());
				newProjectile.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity;
			}
			
			
			
			Destroy (gameObject);
		}
	}
}
