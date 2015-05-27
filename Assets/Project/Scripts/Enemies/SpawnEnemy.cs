using UnityEngine;
using System.Collections;

// Esai - 5/19/2015. Commenting by Matt.
public class SpawnEnemy : MonoBehaviour 
{
	public float spawnRate;				// The rate at which this spawner will spawn a new Enemy.
	public Transform tower;				// Reference to the player tower's location.
	public GameObject enemyPrefab;		// Reference to the Enemy prefab.

	public float timeUntilFirstSpawn = 1f;
	private bool hasFirstEnemySpawned = false;

	float initRate;

	// Use this for initialization
	void Start() 
	{
		initRate = spawnRate;
	}
	
	// Update will handle when to spawn a new enemy based on using a set interval rate.
	void Update() 
	{
		if (hasFirstEnemySpawned == false)
		{
			timeUntilFirstSpawn -= Time.deltaTime;

			if (timeUntilFirstSpawn <= 0f)
			{
				hasFirstEnemySpawned = true;

				// Instantiate the enemy prefab, get its MoveTo script, and set the target to the Player's Tower.
				GameObject newEnemy = (GameObject)Instantiate(enemyPrefab, gameObject.transform.position, enemyPrefab.transform.rotation);
				newEnemy.GetComponent<MoveTo>().target = tower;
				spawnRate = initRate;
			}
		}
		else
		{
			spawnRate -= Time.deltaTime;

			// Time to spawn new Enemy!
			if (spawnRate <= 0) 
			{
				// Instantiate the enemy prefab, get its MoveTo script, and set the target to the Player's Tower.
				GameObject newEnemy = (GameObject)Instantiate(enemyPrefab, gameObject.transform.position, enemyPrefab.transform.rotation);
				newEnemy.GetComponent<MoveTo>().target = tower;
				spawnRate = initRate;
			}
		}
	}
}
