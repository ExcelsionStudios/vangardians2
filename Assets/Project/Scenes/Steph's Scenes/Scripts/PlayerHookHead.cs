using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen - 7/30/15

//NOTE: This script works very closely with PlayerHook.cs 
//      Not meant to be used standalone!

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerHookHead : MonoBehaviour 
{
	public PlayerHook owner;
	//public AnimationCurve 
	public float maxDistance;
	public float speed;
	public Vector3 direction;
	public Transform hit;
	public AnimationCurve extendingMultiplier;
	public float extendingScale = 1.6f;
	public AnimationCurve retractingPattern;
	public float patternScale = 1.6f;
	
	private Vector3 extent = Vector3.zero;
	private float Distance {
		get{
			return Vector3.Distance(owner.transform.position, this.transform.position);
		}
	}

	void OnEnable()
	{
		//StartCoroutine("CollisionTimeout");
		gameObject.layer = LayerMask.NameToLayer("Default");
	}

	//This should only be called from within PlayerHook, as it assumes a few things have been set up already.
	public void Fire( float travelSpeed, float distance )
	{
		speed = travelSpeed;
		maxDistance = distance;
		//We assume PlayerHook moved us so that this calculation will give correct values.
		direction = VectorExtras.Direction(owner.transform.position, transform.position);
		extent = VectorExtras.OffsetPosInDirection( transform.position, direction, distance );
		StartCoroutine( Extend(distance) );
	}

	void OnTriggerEnter2D( Collider2D col ) //This apparently will run even if our gameobject is disabled! (Says on wiki)
	{
		//TODO CHECK IF VALID TARGET HERE!
		if( col.tag == "Enemy" && hit == null )
		{
			Debug.Log("Hookhead hit: " + col.transform.name );
			hit = col.transform;

			//Take control away from the enemy. YOU'RE MINE NOW! >:D
			Enemies.Enemy enemy = col.gameObject.GetComponent<Enemies.Enemy>();
			enemy.Status = Enemies.Situation.BeingPushedByHook;

			//col.gameObject.layer = LayerMask.NameToLayer("Enemy");

			hit.parent = this.transform;
		}
	}

	IEnumerator Extend( float distance )
	{
		Debug.Log("HOOK: Extending...", this);
		StopCoroutine( Retract() );
		gameObject.layer = LayerMask.NameToLayer("Default");

		//As long as our distance has not reached max...
		while( Distance < maxDistance )
		{
			transform.position = Vector3.MoveTowards(transform.position, extent, speed * extendingMultiplier.Evaluate(Distance * extendingScale) * Time.deltaTime);
			yield return new WaitForEndOfFrame(); //TODO change all to: yield return null;
		}

		//At this point, we are fully extended..

		//Do we have an enemy attached? (OnTriggerEnter2D will change hit)
		if( hit == null )
		{
			//No. Start retracting.
			StartCoroutine( Retract() );
		}
		else
		{
			//Yes. Hand over control to our main hook script.
			PassControl();
		}
	}
	IEnumerator Retract()
	{
		Debug.Log("HOOK: Retracting...", this);
		StopCoroutine( Extend(0f) );
		gameObject.layer = LayerMask.NameToLayer("IgnorePhysics");

		while( Distance > 0.65f ) //NOTE that Distance is measured from owner.transform.position and NOT from owner.cannon.position (Which is closer)
		{
			//The below line loops the AnimationCurve infinitely. 'patternScale' changes how often it repeats.
			float pattern = retractingPattern.Evaluate(Mathf.Repeat(Distance * patternScale, 1.0f));

			//transform.position = Vector3.MoveTowards(transform.position, owner.cannon.position, owner.hookSpeed * retractingPattern.Evaluate(progress) * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, owner.cannon.position, owner.hookSpeed * pattern * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		PassControl();
	}

	//Used for situations where the head should reel back in, when it otherwise wouldn't.
	public void ForceRetractFrom( Vector3 position )
	{
		gameObject.layer = LayerMask.NameToLayer("IgnorePhysics");
		StopCoroutine( Extend(0f) );
		StopCoroutine( Retract() );
		transform.position = position;

		StartCoroutine( Retract() );
	}
	private void PassControl() //Pass control to our owner.
	{
		Debug.Log ("Passed control to owner.");
		
		//Reset our values.
		speed = 0f;
		maxDistance = 0f;
		extent = Vector3.zero;
		direction = Vector3.zero;

		owner.OnHeadHit( hit ); //This function will also change the enemy's status appropriately.
		hit = null;
	}


	void OnDisable()
	{
		StopAllCoroutines();
	}





}
