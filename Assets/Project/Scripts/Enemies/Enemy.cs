using System.Collections;
using UnityEngine;

// Matt McGrath 5/19/2015

// Base Enemy class. For now, simply has variables for health and death worth. An Enemy is the basic building block of the game: They are the main threat for the Player 
// and will appear in "Waves," which are then all tied together by a Wave Manager to control the flow of the game.
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
