using UnityEngine;
using System.Collections;

// Matt McGrath - 5/21/2015
public class TrapProjectile : MonoBehaviour 
{
	// For now, instead of trapping one enemy for a set period of time, just make it cause a barrier they can't pass for a period of time (handled via Destroy After Time script.
//	void OnCollisionEnter(Collision col)
//	{
//		// If we collide with an  Enemy, "trap" that Enemy.
//		if(col.gameObject.tag == "Enemy")
//		{
//			Enemy enemy = col.gameObject.GetComponent<Enemy>();
//			MoveTo moveToScript = enemy.GetComponent<MoveTo>();
//			moveToScript.speed = 0f;
//		}
//	}

	public float timeUntilMassIncrease = 3.0f;

	void Update()
	{
		timeUntilMassIncrease -= Time.deltaTime;
	}

	// For now, instead of trapping one enemy for a set period of time, just make it cause a barrier they can't pass for a period of time (handled via Destroy After Time script.
	void OnCollisionEnter(Collision col)
	{
		// If we collide with an  Enemy, "trap" that Enemy.
		if(col.gameObject.tag == "Ground" && timeUntilMassIncrease <= 0f)
		{
			Rigidbody rigidBody = this.gameObject.GetComponent<Rigidbody>();
			rigidBody.mass = 100f;
		}
	}
}
