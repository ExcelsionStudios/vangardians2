using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen 7/27/15

//Main target for enemies to try to carry offscreen.
//

public class Idol : MonoBehaviour 
{
	#region Access anywhere
	private static Idol instance;
	public static Idol Get()
	{
		if( instance != null )
			return instance;
		else 
		{
			GameObject obj = GameObject.FindGameObjectWithTag("Idol");
			if( obj != null )
			{
				Idol script = obj.GetComponent< Idol >() as Idol;
				if( script != null )
				{
					instance = script;
					return instance;
				}
			}
			Debug.LogError ("ERROR: No idol in scene! Put one in!");
			Debug.Break ();
			return null;
		}
	}
	void Awake()
	{
		if( instance == null )
			instance = this;
	}
	#endregion

	//Variables!
	public Light glow;
	public GameObject corpse;


	void Update()
	{
		if( Physics.Raycast(new Vector3(transform.position.x, transform.position.y, 0f), Vector3.forward, 0.1f ) == false )
		{
			Fall();
		}
	}
	void Fall()
	{
		transform.parent = null;
		Drop( null ); //TODO - we need a way to check if we're being held!


		Rigidbody2D r2D = GetComponent<Rigidbody2D>();
		Vector2 velocity = r2D.velocity;
		
		GameObject body = (GameObject)Instantiate( corpse, this.transform.position, this.transform.rotation );
		body.GetComponent<Rigidbody>().velocity = new Vector3( velocity.x, velocity.y, 0.0f ); //Maintain velocity for seamless transition.

		Debug.Log("Idol lost!");
		transform.position = new Vector3( 0.65f, -0.31f, 0f );
	}
	
	public void Pickup( Enemies.Enemy holder )
	{
		gameObject.layer = LayerMask.NameToLayer("IgnorePhysics");
	}

	public void Drop( Enemies.Enemy holder )
	{
		gameObject.layer = LayerMask.NameToLayer("Default");
		transform.position = new Vector3( transform.position.x, transform.position.y, 0.0f );
		transform.rotation = Quaternion.identity;
	}















}
