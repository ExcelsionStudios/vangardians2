using UnityEngine;
using System.Collections;

//Stephan Ennen - 9/21/15

namespace Enemies
{
	public class Spawner : MonoBehaviour 
	{
		public GameObject[] enemyPrefabs;
		public Transform[] spawns;
		public Vector2 spawnDelay = new Vector2(0.5f, 10f);
		public int maxEnemyCount = 10;


		private float timeleft = 0f;
		void Awake()
		{
			if( enemyPrefabs.Length == 0 )
			{
				Debug.LogError("No enemy prefabs given!", this);
				Debug.Break ();
			}
			if( spawns.Length == 0 )
			{
				Debug.LogError("No spawn locations given!", this);
				Debug.Break ();
			}
		}
		void Start()
		{
			StartCoroutine( "SpawnLoop" );
		}


		private IEnumerator SpawnLoop()
		{
			timeleft = Random.Range( spawnDelay.x, spawnDelay.y );

			while( timeleft > 0.0f )
			{
				while( timeleft < spawnDelay.x && Enemy.count >= maxEnemyCount )
				{
					timeleft = spawnDelay.x;
					yield return null;
				}

				timeleft -= Time.deltaTime;
				yield return null;
			}

			Spawn();
			StartCoroutine( "SpawnLoop" );
		}

		void Spawn()
		{
			Transform location = ArrayTools.Shuffle<Transform>( spawns )[0];
			GameObject prefab = ArrayTools.Shuffle<GameObject>( enemyPrefabs )[0];

			GameObject.Instantiate(prefab, location.position, Quaternion.identity);
		}


		
	}
}