using UnityEngine;
using System.Collections;

// Matt McGrath - 5/21/2015
public class TrapProjectile : MonoBehaviour 
{
	void OnCollisionEnter(Collision col)
	{
		// If we collide with an  Enemy, "trap" that Enemy.
		if(col.gameObject.tag == "Enemy")
		{
			Enemy enemy = col.gameObject.GetComponent<Enemy>();
			MoveTo moveToScript = enemy.GetComponent<MoveTo>();
			moveToScript.speed = 0f;
		}
	}
}
