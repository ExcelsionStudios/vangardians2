using System.Collections;
using UnityEngine;

//Script by Esai Solorio
//June 16, 2015

// Simple Script that attaches an enemy as a child of the hook.
public class HookEnemy : MonoBehaviour 
{
	public HookShot hookShot;
	public GameObject haloPrefab;

	void OnTriggerEnter(Collider col)
	{
		// If we enter collision with an Enemy tag and aren't yet hooked to an enemy...
		if (col.gameObject.tag == "Enemy" && !hookShot.enemyHooked) 
		{
			// Set the collided Enemy's parent as this object's transform.
			col.gameObject.transform.parent = gameObject.transform;
			col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

			// We don't want the Enemy to still be able to move: He's stuck by the hook!
			col.gameObject.GetComponent<MoveTo>().enabled = false;

			GameObject myHalo = (GameObject)Instantiate(haloPrefab, col.gameObject.transform.position, haloPrefab.transform.rotation);
			myHalo.transform.parent = col.gameObject.transform;
			hookShot.HookEnemy(col.gameObject);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Enemy") 
		{
			hookShot.hookedObject.GetComponent<Rigidbody>().isKinematic = false;	// Matt: This line gives NullReferenceException. Seems to be when a Hooked Enemy touches another enemy.
			hookShot.hookedObject = null;
			
			foreach (Transform child in col.gameObject.transform) 
			{
				Destroy(child.gameObject);
			}
		}
	}
}
