using UnityEngine;
using System.Collections;

//Stephan Ennen - 6/24/15

//NOTE: This script works very closely with SimpleHook.cs 
//      Not meant to be used standalone!

[RequireComponent(typeof(CircleCollider2D))]
public class SimpleHookHead : MonoBehaviour 
{
	public SimpleHook owner;
	//public AnimationCurve 
	public float maxDistance;
	public float speed;
	public Vector3 direction;
	public Transform hit;
	
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

	//This should only be called from within SimpleHook, as it assumes a few things have been set up already.
	public void Fire( float travelSpeed, float distance )
	{
		speed = travelSpeed;
		maxDistance = distance;
		//We assume SimpleHook moved us so that this calculation will give correct values.
		direction = VectorExtras.Direction(owner.transform.position, transform.position);
		extent = VectorExtras.OffsetPosInDirection( transform.position, direction, distance );
		StartCoroutine( Extend() );
	}

	void OnTriggerEnter2D( Collider2D col ) //This apparently will run even if our gameobject is disabled! (Says on wiki)
	{
		//TODO CHECK IF VALID TARGET HERE!
		if( col.tag == "Enemy" && hit == null )
		{
			Debug.Log("Hookhead hit: " + col.transform.name );
			hit = col.transform;

			SimpleHook.SetEnemyControl(col.gameObject, false); //Take control away from the enemy. YOU'RE MINE NOW! >:D
			col.gameObject.layer = LayerMask.NameToLayer("Enemy");

			hit.parent = this.transform;
		}
	}

	void Update () 
	{
		//Debug.Log( Distance );
	}

	IEnumerator Extend()
	{
		Debug.Log("HOOK: Extending...", this);
		StopCoroutine( Retract() );
		gameObject.layer = LayerMask.NameToLayer("Default");

		//As long as our distance has not reached max...
		while( Distance < maxDistance )
		{
			transform.position = Vector3.MoveTowards(transform.position, extent, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame(); //TODO change all to: yield return null;
		}

		//At this point, we are fully extended..

		//Do we have an enemy attached?
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
		StopCoroutine( Extend() );
		gameObject.layer = LayerMask.NameToLayer("IgnorePhysics");
		
		while( Distance > 0.65f ) //NOTE that distance is measured from owner.transform.position and NOT from owner.cannon.position (Which is closer)
		{
			//VectorExtras.Direction(transform.position,owner.transform.position) * owner.hookSpeed * Time.deltaTime
			transform.position = Vector3.MoveTowards(transform.position, owner.cannon.position, owner.hookSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		PassControl();
	}
	private void PassControl() //Pass control to our owner.
	{

		if( hit != null )
		{
			SimpleHook.SetEnemyControl(hit.gameObject, true); //Give the enemy control back. (SimpleHook will immediately take control)
		}


		//Reset our values.
		speed = 0f;
		maxDistance = 0f;
		extent = Vector3.zero;
		direction = Vector3.zero;

		owner.OnHeadHit( hit );
		hit = null;
	}


	void OnDisable()
	{
		StopAllCoroutines();
	}





}
