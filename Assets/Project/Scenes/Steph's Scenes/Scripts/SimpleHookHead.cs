using UnityEngine;
using System.Collections;

//Stephan Ennen - 6/18/15

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleHookHead : MonoBehaviour 
{
	public SimpleHook owner;
	public float distanceTimeout;
	
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
	
	void Update () 
	{
		//Distance timeout.
		if( distanceTimeout > 0f )
		{
			if( Vector3.Distance(owner.transform.position, this.transform.position) > distanceTimeout )
			{
				//Timeout!
				owner.OnHeadHit( null );
			}
		}
	}







}
