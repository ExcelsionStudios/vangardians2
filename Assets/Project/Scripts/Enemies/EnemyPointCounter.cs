using System.Collections;
using UnityEngine;

// Matt McGrath - 5/19/2015.

namespace Enemies 
{
	// Script which we will temporarily attach to the SlingProjectile to keep track of "Score."
	public class EnemyPointCounter : MonoBehaviour 
	{
		private static int score;
		
		// Get and set the Score value. Public set because we now want Enemies to reduce the Player's score.
		public static int Score
		{
			get { return score; }
		    set { score = Mathf.Clamp(value, 0, int.MaxValue); }
		}

		void OnCollisionEnter(Collision col)
		{
			// If the collision was with an Enemy tagged object...
			if (col.gameObject.tag == "Enemy")
			{
				// In this prototype, we assume (for now) that a collision with the projectile results in death. Later we will obviously need to see if the enemy's health is < 0.
				//score += col.gameObject.GetComponent<Enemy>().KillPoints;
			}
		}
	}
}