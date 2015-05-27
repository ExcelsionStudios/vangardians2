using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Effects
{
	public class ExplodeOnEnemyImpact : MonoBehaviour 
	{
		public GameObject explodePrefab;
	
		// Use this for initialization
		void OnCollisionEnter(Collision col)
		{
			if (col.gameObject.tag == "Enemy") 
			{
				Destroy (gameObject);
				GameObject newExplosion = (GameObject)Instantiate (explodePrefab, gameObject.transform.position, explodePrefab.transform.rotation);
				newExplosion.GetComponent<ParticleSystemMultiplier>().multiplier = .25f;
				newExplosion.GetComponent<ExplosionPhysicsForce>().explosionForce = 8;

				// Don't Destroy anymore! Enemies now have health. Destroying will occur in the DamageEnemy script.
				//Destroy(col.gameObject);
			}
		}
	}
}