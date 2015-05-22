using UnityEngine;
using System.Collections;

public class PlayerTower : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

//	void OnCollisionEnter(Collision col)
//	{
//		// If we collide with an  Enemy, Destroy that Enemy.
//		if (col.gameObject.tag == "Enemy")
//		{
//			Debug.Log ("Enemy Entered Tower!");
//			Enemy enemy = col.gameObject.GetComponent<Enemy>();
//			EnemyPointCounter.Score -= enemy.KillPoints / 4;
//			Destroy(col.gameObject);
//		}
//	}

	void OnTriggerEnter(Collider other)
	{
		// If we collide with an  Enemy, Destroy that Enemy.
		if (other.gameObject.tag == "Enemy")
		{
			Debug.Log ("Enemy Entered Tower!");
			Enemy enemy = other.gameObject.GetComponent<Enemy>();
			EnemyPointCounter.Score -= enemy.KillPoints / 4;
			Destroy(other.gameObject);
		}
	}
}
