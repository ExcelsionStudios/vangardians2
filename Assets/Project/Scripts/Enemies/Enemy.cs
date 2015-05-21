using UnityEngine;
using System.Collections;

// Matt McGrath 5/19/2015

// Enemy class. Does nothing now but contain how many points its death is worth.
public class Enemy : MonoBehaviour 
{
	public float MaxHealth;

	public float Health
	{
		get { return health; }
		set { health = Mathf.Clamp(value, 0f, MaxHealth); }
	}
	private float health;

	public int KillPoints;


	void Start()
	{
		Health =  MaxHealth;
	}
}
