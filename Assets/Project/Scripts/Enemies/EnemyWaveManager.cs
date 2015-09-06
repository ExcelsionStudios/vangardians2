using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Matt McGrath, 5/23/2015

namespace Enemies 
{
	// The Wave Manager manages how Waves of Enemies are handled. Here we can determine what events / enemies appear during which Waves.
	public class EnemyWaveManager : MonoBehaviour 
	{
		// A list of Waves the Manager is controlling.
		public List<EnemyWave> waves;

		// The number of the current wave.
		public int CurrentWaveNumber;


		// BEING LAZYYYYY: For Prototype, let's Prefab "Waves" and drag them in here.  Can be a list or array later.
		public List<GameObject> prefabedWaves;
		public EnemyWave currentPrefabedWave;


		private float gracePeriodBetweenWaves = 5.0f;
		private float gracePeriodTimer;

		// Use this for initialization
		void Start () 
		{
			// We start on Wave 1.
			CurrentWaveNumber = 1;

			SetUpPrefabedWave(CurrentWaveNumber);
		}
		
		// Update is called once per frame
		void Update () 
		{
			//if (waves[CurrentWaveNumber - 1].IsWaveComplete)
			{
				// Some pausing? Some menu? 

				// Give the player a chance to spend his Money on upgrades?

				// Cleanup the previous wave, in case of memory leaks.

				//Then go onto the next wave.
			}

			if (currentPrefabedWave != null && currentPrefabedWave.IsWaveComplete)
			{
				Debug.Log ("Wave Complete! New One Starts In " + gracePeriodTimer);
				gracePeriodTimer -= Time.deltaTime;
				if (gracePeriodTimer <= 0f)
				{
					currentPrefabedWave.gameObject.SetActive(false);
					Destroy(currentPrefabedWave);

					SetUpPrefabedWave(++CurrentWaveNumber);
				}
			}
		}

		void SetUpPrefabedWave(int waveNumber)
		{
	//
	//		if (waveNumber >= prefabedWaves.Count);
	//		    return;

			Debug.Log("Setting Up Next Wave");
			GameObject go = prefabedWaves[waveNumber - 1];
			currentPrefabedWave = go.GetComponent<EnemyWave>();
			currentPrefabedWave.gameObject.SetActive(true);
		
			gracePeriodTimer = gracePeriodBetweenWaves;

			currentPrefabedWave.BeginWave();
		}

		// Sets up the Enemy Wave number specified in the parameter. For now let's use a switch-case to keep things simple.
		void SetUpWave(int waveNumber)
		{
			EnemyWave wave = new EnemyWave();

			switch (waveNumber)
			{
			case 1:
				wave.TotalNumberOfEnemies = 5;
				wave.WaveTime = 30f;
				break;
			case 2:
				wave.TotalNumberOfEnemies = 10;
				wave.WaveTime = 60f;
				break;
			case 3:
				wave.TotalNumberOfEnemies = 15;
				wave.WaveTime = 90f;
				break;
			default:
				wave.TotalNumberOfEnemies = 10;
				wave.WaveTime = 60f;
				break;
			}

			waves.Add(wave);
		}
	}
}
