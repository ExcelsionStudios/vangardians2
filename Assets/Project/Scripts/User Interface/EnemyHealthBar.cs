using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Matt McGrath 5/27/2015

namespace Enemies 
{
	// Simple script which should be attached only to Enemy prefabs that contain an Enemy script. This will control the prefab's slider and use it as a health bar.
	public class EnemyHealthBar : MonoBehaviour 
	{
		public Slider healthSlider;				// Reference to the slider that will act as the health bar.

		private Enemy enemyReference;

		// Use this for initialization
		void Start () 
		{
			// We KNOW (or should) that this script is attached to an Enemy prefab with an Enemy script, so grab a reference to it.
			enemyReference = this.gameObject.GetComponent<Enemy>();
		}
		
		// Update is called once per frame
		void Update () 
		{
			// Update the slider to have a value matching the Enemy's health.
			healthSlider.value = enemyReference.Health;
		}
	}
}
