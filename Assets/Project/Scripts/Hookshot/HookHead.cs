using UnityEngine;
using System.Collections;

// Stephan - Around 6/07/2015

[RequireComponent(typeof(Rigidbody2D))]
public class HookHead : MonoBehaviour 
{
	public Hook owner;

	void OnEnable()
	{
		StartCoroutine("CollisionTimeout");
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}

	void OnCollisionEnter2D( Collision2D col )
	{
		//CHECK IF ENEMY HERE!
		owner.OnHeadHit( col.transform );
	}

	IEnumerator CollisionTimeout()
	{
		yield return new WaitForSeconds( 1.0f );
		owner.OnHeadHit( null );
	}

	void Start () 
	{
	}

	void Update () 
	{
	}
}
