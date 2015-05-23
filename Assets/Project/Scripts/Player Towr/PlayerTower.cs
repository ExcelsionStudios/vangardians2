using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Matt McGrath 5/21/2015

// The PlayerTower is the building the player is both controlling and protecting.
// Controlling this tower's projectiles currently takes place through the ProjectileSpawnerPrediction class.
public class PlayerTower : MonoBehaviour 
{
	FlingProjectilePrediction projectileLauncher;

	// Health represents the Tower's condition. When it reaches 0, it breaks. Or something. Stuff will happen.
	public float MaxHealth = 100f;
	public float Health
	{
		get { return Mathf.Clamp(health, 0f, MaxHealth); }
		set { health = value; }
	}
	private float health;

	// Mana represents the player's expendable resource pool that is utilized by the projectiles he launches.
	public float MaxMana = 100f;
	public float Mana
	{
		get { return Mathf.Clamp(mana, 0f, MaxMana); }
		set { mana = value; }
	}
	private float mana;

	// Quick-and-dirty Text to show the Tower's Health and Mana. Set this in inspector. TODO Health Meter?
	public Text healthText;
	public Text manaText;

	// More quick and dirtiness to show a game over type screen. But the game will still run after game over is reached.
	public CanvasGroup gameOverUIReference;

	// Rate at which Mana recharges. This will be amount of mana per second.
	public float ManaRechargeRate = 2f;

	void OnAwake()
	{
	}
	// Use this for initialization: Simply sets Health and Mana to their max values for now.
	void Start()
	{
		Debug.Log ("Health: " + Health + ", health: " + health);
		Debug.Log ("Mana: " + Mana + ", health: " + mana);
		Health =  MaxHealth;
		Mana = MaxMana;
		Debug.Log ("Mana: " + Mana + ", health: " + mana);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Update the health text's text value to show current health.
		healthText.text = "HP: " + (int)Health + " / " + (int)MaxHealth;
		manaText.text = "Mana: " + (int)Mana + " / " + (int)MaxMana;

		// Recharge the player's Mana pool.
		Mana += ManaRechargeRate * Time.deltaTime;

		if (health <= 0f)
		{
			gameOverUIReference.alpha = 1;
			gameOverUIReference.interactable = true;
			gameOverUIReference.blocksRaycasts = true;
		}
	}

	// ** ALTER THIS! ** We don't want enemies just disappearing anymore. We want them to linger and attack the Player Tower.
	void OnTriggerEnter(Collider other)
	{
		// If we collide with an  Enemy, Destroy that Enemy.
		if (other.gameObject.tag == "Enemy")
		{
		//	Debug.Log ("Enemy Entered Tower!");
		//	Enemy enemy = other.gameObject.GetComponent<Enemy>();
		//	EnemyPointCounter.Score -= enemy.KillPoints / 4;
		//	Destroy(other.gameObject);
		}
	}
}
