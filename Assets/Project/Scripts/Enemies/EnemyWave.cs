using System.Collections;
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

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
