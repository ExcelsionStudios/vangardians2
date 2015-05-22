using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Matt McGrath 5/21/2015

// The PlayerTower is the building the player is both controlling and protecting.
// Controlling this tower currently takes place through the ProjectileSpawnerPrediction class.
public class PlayerTower : MonoBehaviour 
{
	public float MaxHealth;
	
	public float Health
	{
		get { return health; }
		set { health = Mathf.Clamp(value, 0f, MaxHealth); }
	}
	private float health;

	// Quick-and-dirty Text to show the Tower's Health. Set this in inspector. TODO Health Meter?
	public Text healthText;

	// Use this for initialization: Simply sets Health to Max for now.
	void Start()
	{
		Health =  MaxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Update the health text's text value to show current health.
		healthText.text = Health + " / " + MaxHealth;
	}

	// ** ALTER THIS! ** We don't want enemies just disappearing anymore. We want them to linger and attack the Player Tower.
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
