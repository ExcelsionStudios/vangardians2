using System.Collections;
using UnityEngine;

// Matt McGrath - 5/18/2015.

// Script which we will temporarily attach to the SlingProjectile to keep track of "Kills."
public class DestroySelfOnImpact : MonoBehaviour 
{
	private static uint numberOfKills;
	
	// We want access to this value, but we don't want to allow other classes to change its value.
	public static uint NumberOfKills
	{
		get { return numberOfKills; }
		private set { numberOfKills = value; }
	}

	void OnCollisionEnter(Collision col)
	{
//		Debug.Log(this.gameObject.tag + " collided with " + col.gameObject.tag);

		// Don't get destroyed if you're a projectile hitting another projectile.
		if (this.gameObject.tag == "Projectile" && col.gameObject.tag == "Projectile")
		{
			// Don't destroy.
		}

		// Don't get destroyed if we hit our own "Player" (Tower, really).
		else if (col.gameObject.tag == "PlayerTower")
		{
			// Don't destroy.
		}

		else
		{
			Destroy(gameObject);
		}
	}
}
