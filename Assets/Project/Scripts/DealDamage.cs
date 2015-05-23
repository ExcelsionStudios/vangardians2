using UnityEngine;
using System.Collections;
// Written by Esai Solorio, May 22, 2015
// Small additions by Matt McGrath - 5/22/2015

// Script which we will attach to Enemies so they deal damage to the tower. TODO: Maybe just put this in the Enemy class.
public class DealDamage : MonoBehaviour 
{
	// Use this for initialization
	public float power;
	public float rate;
	
	// Reference to our PlayerTower script.
	private PlayerTower playerTowerReference;

	void Start()
	{
	//	gameObject.GetComponent<Animation> () ["Attack"].speed = rate;
		Animator animator = gameObject.GetComponent<Animator>();
		if (animator == null)
			Debug.Log ("Null Animator");
		else
			animator.speed = rate;

		// Cache the reference to the PlayerTower component. We don't want GetComponent being called often: It's expensive!
		playerTowerReference = transform.parent.GetComponent<MoveTo>().target.GetComponent<PlayerTower>();
		//playerTowerReference = this.gameObject.GetComponent<MoveTo>().target.GetComponent<PlayerTower>();		// If Enemies end up not being parented
	}

	// Take health away from the player's Tower based on the power specified.
	public void doDamage()
	{
		playerTowerReference.Health -= power;
	}
}
