using UnityEngine;
using System.Collections;

// Script which will destroy the object it's attached to. Technically only if it hits an Enemy, so this could probably be renamed to something more fitting. "DestroyEnemyOnImpact".
public class DestroyOnImpact : MonoBehaviour 
{
	void OnCollisionEnter(Collision col)
	{
		// If we collide with an  Enemy, Destroy that Enemy.
		if(col.gameObject.tag == "Enemy")
		{
			Destroy(col.gameObject);
		}
	}
}
