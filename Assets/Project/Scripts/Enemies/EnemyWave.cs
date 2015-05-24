﻿using System.Collections;
using UnityEngine;

// Matt McGrath - 5/23/2015

// An Enemy Wave consists of Enemies which are spawned each new "level" or "wave" or etc. They come in waves which dictate how many enemies there will be, when they will spawn, where they 
// will spawn, what type of enemies will appear, etc. 
using System.Collections.Generic;


public class EnemyWave : MonoBehaviour 
{
	public List<Enemy> activeEnemies;		// List of Enemy objects that have spawned already (and are still alive).
	public int TotalNumberOfEnemies;		// Number of total enemies that will spawn this wave.
	public int NumberOfEnemiesSpawned;		// Number of enemies that have already spawned.
	public bool IsWaveComplete;				// TODO: Make this based on...something.
	public float WaveTime;					// How long (in seconds) will wave last? (TODO: Or will the wave last / end based on other conditions? Design info needed)
	private float waveTimeRemaining;		// How long (in seconds) until the wave is ending (stopping enemies from being spawned).
	public GameObject[] SpawnPoints;		// An array storing all possible points enemies can spawn from.
	public float SpawnRate;					// The rate (in seconds) at which a new enemy will spawn. (TODO: Perhaps each spawn point hsa this attribute, maybe make Spawners its own small class).
	private float spawnRateTimer;			// Timer helper that counts down from SpawnRate.
	private int previousSpawn;

	public int ScoreThisWave;				// Current Score Player has earned during this wave. Perhaps we'll display and rank him after each wave.
	public bool IsWaveInProgress;			// Is the wave currently in progress, or are we in between waves (perhaps doing post-wave stuff like Upgrading)




	// Use this for initialization
	void Start () 
	{
		foreach (GameObject spawner in SpawnPoints)
		{
			spawner.SetActive(true);
		}

		IsWaveInProgress = true;

		waveTimeRemaining = WaveTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!IsWaveInProgress)
			return;

		//spawnRateTimer += 1.0f * Time.deltaTime;
		waveTimeRemaining -= 1.0f * Time.deltaTime;

		Debug.Log ("Wave Time Remaining: " + waveTimeRemaining);
		
//		if (spawnRateTimer >= SpawnRate)
//		{
//			spawnRateTimer = 0.0f;
//			SpawnEnemy();
//		}

		if (waveTimeRemaining <= 0f)
		{
			EndWave();
		}

	}

	// TODO: Change EnemySpawner or utilize it here.
	void SpawnEnemy()
	{

	}

	void EndWave()
	{
		IsWaveInProgress = false;
		IsWaveComplete = true;
		this.gameObject.SetActive(false);
		Destroy(this.gameObject, 0.5f);
	}

	void BeginWave()
	{
		IsWaveInProgress = true;
		IsWaveComplete = false;
	}
}
