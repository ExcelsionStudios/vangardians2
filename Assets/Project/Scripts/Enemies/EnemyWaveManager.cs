using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Matt McGrath, 5/23/2015

// The Wave Manager manages how Waves of Enemies are handled. Here we can determine what events / enemies appear during which Waves.
public class EnemyWaveManager : MonoBehaviour 
{
	// A list of Waves the Manager is controlling.
	public List<EnemyWave> waves;

	// The number of the current wave.
	public int CurrentWaveNumber;

	// Use this for initialization
	void Start () 
	{
		// We start on Wave 1.
		CurrentWaveNumber = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (waves[CurrentWaveNumber - 1].IsWaveComplete)
		{
			// Some pausing? Some menu? 

			// Give the player a chance to spend his Money on upgrades?

			// Cleanup the previous wave, in case of memory leaks.

			//Then go onto the next wave.

		}
	}

	// Sets up the Enemy Wave number specified in the parameter. For now let's use a switch-case to keep things simple.
	void SetUpWave(int waveNumber)
	{
		EnemyWave wave = new EnemyWave();

		switch (waveNumber)
		{
		case 1:
			wave.TotalNumberOfEnemies = 5;
			break;
		case 2:
			wave.TotalNumberOfEnemies = 10;
			break;
		case 3:
			wave.TotalNumberOfEnemies = 15;
			break;
		case 4:
			wave.TotalNumberOfEnemies = 20;
			break;
		default:
			wave.TotalNumberOfEnemies = 10;
			break;
		}
	}
}
