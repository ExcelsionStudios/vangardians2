using UnityEngine;
using System.Collections;

// Matt McGrath 5/21/2015

// Script which will destroy the object it's attached to. Technically only if it hits an Enemy, so this could probably be renamed to something more fitting. "DestroyEnemyOnImpact".
public class DamageEnemyOnImpact : MonoBehaviour 
{
	void OnCollisionEnter(Collision col)
	{
		// If we collide with an  Enemy, Destroy that Enemy.
		if(col.gameObject.tag == "Enemy")
		{
			Enemy enemy = col.gameObject.GetComponent<Enemy>();
			//Debug.Log("Enemy with " + enemy.Health + "Health hit for " + this.gameObject.GetComponent<Projectile>().Damage.ToString () + " Damage!");
			enemy.Health -= this.gameObject.GetComponent<Projectile>().Damage;

			if (enemy.Health <= 0f)
			{
				PlayerTower.NumberOfKills++;
				Destroy (col.transform.parent.gameObject);
				//Destroy (col.gameObject);			// If the Enemy is only a single gameObject like before.
			}
		}
	}
}
