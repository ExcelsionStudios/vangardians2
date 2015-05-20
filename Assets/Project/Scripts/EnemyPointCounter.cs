using System.Collections;
using UnityEngine;

// Matt McGrath - 5/19/2015.

// Script which we will temporarily attach to the SlingProjectile to keep track of "Score."
public class EnemyPointCounter : MonoBehaviour 
{
	private static uint score;
	
	// We want access to this value, but we don't want to allow other classes to change its value.
	public static uint Score
	{
		get { return score; }
		private set { score = value; }
	}

	void OnCollisionEnter(Collision col)
	{
		// If the collision was with an Enemy tagged object...
		if (col.gameObject.tag == "Enemy")
		{
			// In this prototype, we assume (for now) that a collision with the projectile results in death. Later we will obviously need to see if the enemy's health is < 0.
			score += col.gameObject.GetComponent<Enemy>().KillPoints;
		}
	}
}
