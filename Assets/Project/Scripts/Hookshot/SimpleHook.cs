using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Enemies;
using Enemies.Modules;

//Stephan Ennen - 7/27/15

//NOTE: This script works very closely with SimpleHookHead.cs 
//      Not meant to be used standalone!

public class SimpleHook : MonoBehaviour 
{
	public SimpleHookHead head;
	public LineRenderer chain;		//This is also what we draw our chain from.
	private Transform c; 			//Storage for the below accessor. (only use the below and ignore this)
	public Transform connection { 	//This is what we draw our chain to.
		get{ return c; }
		set{
			if( hookedEnemy != null )
				hookedEnemy.Status = Situation.InControl; //hookedEnemy.isHooked = false;
			c = value;
		}
	}
	public Transform cannon; 				//Our cannon head. We rotate this.
	public Transform trajectoryVisual; 
	public Transform swipeDetector; 		//Helps detect swipes.
	public Transform swingDetector; 		//Helps detect if we're swinging or slamming.

	public float hookSpeed; 				//speed our hook shoots out, or reels back in.
	public float maxHookDistance = 4f;  	//Max distance our hook will travel.
	public float slamVelocity = 10.0f;    	//Essentially the animation Speed when slamming an enemy.
	public float slamSensitivity = 90.0f; 	//If our delta angle is larger than this while dragging, it triggers a slam.

	public AnimationCurve enemyScaling; 	//Scale multiplier of enemy as they are tossed.

	private Vector2 startDragPos = Vector2.zero;
	public Vector2[] path = new Vector2[0]; 
	public bool[] wallTouches = new bool[0];
	float totalAngle = 0f;
	public Enemies.Enemy hookedEnemy;

	void Start () 
	{
		if (head != null)
			head.gameObject.SetActive(false);
		if (trajectoryVisual != null)
			trajectoryVisual.gameObject.SetActive(false);
	}

	public float dragSmoothing;
	private Vector3 smoothedDragPos = Vector3.zero;
	private Vector3 smoothedVel;

	void OnDrawGizmosSelected()
	{
		if( !Application.isEditor || !Application.isPlaying )
			return; //Code throws array index errors because it tries to run when the game isnt.

		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere( smoothedDragPos, 0.15f );

		//Draw valid touch area.
		//Gizmos.color = Color.white;
		//Gizmos.DrawWireSphere( 

		/*
		Gizmos.color = Color.green;
		if( path != null && path.Length > 0 )
		{
			for( int i = 0; i < path.Length; i-- )
			{
				Debug.LogError("I: "+ i +", length: "+ (path.Length-1));
				Vector3 start;
				Vector3 end;
				if( i == path.Length - 1 )
					start = new Vector3(VectorExtras.GetMouseWorldPos().x, VectorExtras.GetMouseWorldPos().y);
				else
					start = new Vector3(path[i+1].x, path[i+1].y);
				end = new Vector3(path[i].x, path[i].y);
				Gizmos.DrawLine( start, end );
			}
		} */
	}

	void Update() 
	{

		//Vector2 pushedPos = VectorExtras.OffsetPosInPointDirection( VectorExtras.V2FromV3(transform.position), VectorExtras.GetMouseWorldPos(), 3f );
		//Vector2 pushedPos = VectorExtras.MinAnchoredMovePosTowardTarget(VectorExtras.V2FromV3(transform.position), VectorExtras.GetMouseWorldPos(), 3f, 3f );
		smoothedDragPos = Vector3.SmoothDamp(smoothedDragPos, VectorExtras.V3FromV2(VectorExtras.GetMouseWorldPos(),0f), ref smoothedVel, dragSmoothing, 9999f);
		if( startDragPos != Vector2.zero )
		{
			Vector2 direction = VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), VectorExtras.V2FromV3(smoothedDragPos) );
			TransformExtensions.SetRotation2D( swipeDetector, VectorExtras.VectorToDegrees(direction) );
			//Debug.Log( smoothedVel.magnitude );
		}

		//swipeDetector


		if( inSlam == false )
			ReadInput();
		
		//Update our line renderer.
		if (connection != null)
		{
			chain.SetPosition(1, chain.transform.position); // This order makes it appear that chain comes out of the cannon.
			chain.SetPosition(0, connection.position);
			chain.material.mainTextureScale = new Vector2(Vector3.Distance(chain.transform.position, connection.position), 1f);
		}
		else
		{
			chain.SetPosition(1, Vector3.zero);
			chain.SetPosition(0, Vector3.zero);
			//Setting the texture scale doesn't matter here.
		}
	}


	void ReadInput()
	{
		if (connection == null) // Our head is not present and there is no enemy attached. (Do code for a new head)
		{
			if (startDragPos == Vector2.zero)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Vector2 mPos = VectorExtras.GetMouseWorldPos();
					if (mPos.magnitude < 1.25f)
					{
						startDragPos = mPos;
					}
				}
			}
			else
			{
				Vector2 mPos = VectorExtras.GetMouseWorldPos();

				if (Input.GetMouseButton(0)) // Player is aiming (We're about to fire the hook)
				{
					Vector2 direction = -VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), mPos );
					TransformExtensions.SetRotation2D( cannon, VectorExtras.VectorToDegrees(direction) );

					//Draw the trajectory indicator
					trajectoryVisual.gameObject.SetActive( true );
					Vector2 target = direction * Vector3.Distance(transform.position, new Vector3( mPos.x, mPos.y, 0f ));
					trajectoryVisual.position = new Vector3( target.x, target.y, -0.001f );

				}
				else if (Input.GetMouseButtonUp(0)) // Player let go. Fire the hook.
				{
					trajectoryVisual.gameObject.SetActive( false );
					
					// Matt: Added this -- was getting errors when messing around. TODO: Ensure valid head since this will ruin further calculations.
					if (head != null)
					{
						head.gameObject.SetActive(true);
						head.transform.position = VectorExtras.V3FromV2( -VectorExtras.OffsetPosInPointDirection( VectorExtras.V2FromV3(transform.position), mPos, 0.61f ), 0.0f );
						head.Fire(hookSpeed, Mathf.Min(Vector3.Distance(VectorExtras.V3FromV2( mPos, 0f ), this.transform.position), maxHookDistance));

						connection = head.transform;
					}
					
					startDragPos = Vector2.zero;
				}
			}
		}
		else // Either an enemy is attached or a head is.
		{
			// Always look at our connection.
			// Vector2 direction = -VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), VectorExtras.V2FromV3(connection.position) );
			TransformExtensions.SetRotation2D(cannon, VectorExtras.VectorToDegrees(VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), VectorExtras.V2FromV3(connection.position))));
			
			if (head.isActiveAndEnabled == true)
			{
				//Do nothing (for now)
			}
			else
			{
				//The head is not present. We must have an enemy. 
				//TODO: we can directly check this now. Look for Enemy component on object.
				if (startDragPos == Vector2.zero)
				{
					if (Input.GetMouseButtonDown(0))
					{
						Vector2 mPos = VectorExtras.GetMouseWorldPos();
						if (Vector2.Distance(mPos, VectorExtras.V2FromV3(connection.position)) < 3.0f) //Make sure we're starting the click somewhat close to our hooked enemy. TODO make public var
						{
							startDragPos = mPos;

							path = new Vector2[0];
							path = ArrayTools.PushLast<Vector2>(path, mPos);
							wallTouches = new bool[0];
							wallTouches = ArrayTools.PushLast<bool>(wallTouches, GetMouseHitSides(mPos));

							totalAngle = 0f;
						}
					}
				}
				else
				{
					Vector2 mPos = VectorExtras.GetMouseWorldPos();
					if( Input.GetMouseButton(0) ) //Player is holding..
					{
						//http://gamedevelopment.tutsplus.com/tutorials/how-to-detect-when-an-object-has-been-circled-by-a-gesture--gamedev-336

						//Keeping this in case we want to be able to "throw" an enemy in a circle later.
						Vector2 lastPos = path[path.Length-1];
						path = ArrayTools.PushLast<Vector2>(path, mPos); //TODO this array will get quite large for extended touches!
						wallTouches = ArrayTools.PushLast<bool>(wallTouches, GetMouseHitSides(mPos));
						
						//We calculate the angle from last frame's position to this frame's position, then add it to totalAngle.
						Vector2 prev = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), lastPos); //This will cause problems if the player moves!
						Vector2 curr = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), mPos);
						float deltaAng = Vector2.Angle( prev, curr );
						totalAngle += deltaAng;


						//Look to see if our detlta angle was of a certian size. 
						//If large enough, we can assume that the path was a slam. Forcefully end the touch logic if this is the case.
						//TODO This detection is vulnerable to lag!!!! (Add some sort of time.deltatime)
						//TODO make this detection more accurate.

						bool noTouchy = false;
						for( int i = Mathf.Max((path.Length - 5), 0); i < path.Length; i++ ) //Search back up to 4 frames.
						{
							if( wallTouches[i] == false )
								continue;
							else
							{
								noTouchy = true;
								break;
							}
						}
						if( noTouchy == false )
						{
							//Debug.Log("Has not touched wall!");

							if( swipeDetector.GetComponentInChildren<CircleCollider2D>().OverlapPoint( mPos ) )
							{
								if( smoothedVel.magnitude > 5.0f )
								{
									StartCoroutine( Slam() );
									return;
								}
							}
							
							/*if( deltaAng > slamSensitivity )
							{
								StartCoroutine( Slam() );
								return;
							} */
						}




						//Move our enemy.
						//Vector3 mousePosV3 = VectorExtras.V3FromV2( mPos, 0f );
						//TODO add smoothing. TODO store a velocity variable //TODO track touch offset at beginning of touch.
						//connection.transform.position = VectorExtras.OffsetPosInPointDirection(transform.position, mousePosV3, Vector3.Distance(transform.position, connection.position)); 
						connection.transform.position = VectorExtras.OffsetPosInPointDirection(transform.position, smoothedDragPos, Vector3.Distance(transform.position, connection.position)); 



					}
					else if (Input.GetMouseButtonUp(0)) //Player let go.
					{
						path = ArrayTools.PushLast<Vector2>(path, mPos);
						wallTouches = ArrayTools.PushLast<bool>(wallTouches, GetMouseHitSides(mPos));

						//Apply our swing physics here if we want it.



						
						startDragPos = Vector2.zero;
					}
				}
			}
		}
	}

	private bool inSlam = false; //DONT use enum for this. We use this for this script, not our enemy.
	IEnumerator Slam() //Animate our slam, then do damage to enemies in radius.
	{
		//SlamEvent is any custom behaviours the enemy has defined. (Such as taking damage, or exploding..)
		SlamBehaviour slamEvent = connection.GetComponent<SlamBehaviour>();
		if( slamEvent != null )
			slamEvent.OnSlamStart();
		else
			Debug.LogWarning("Enemy has no SlamBehaviour component. Slamming this enemy wont do much.", this);

		startDragPos = Vector2.zero;
		inSlam = true;
		connection.parent = cannon;

		slamEvent.owner.Status = Situation.BeingSlammed;

		float t = 0f;
		while( t < 180f )
		{

			//TODO make this not able to overshoot, add Time.deltaTime multiplier
			t += slamVelocity;
			t = Mathf.Clamp(t, 0f, 180.0f);
			float progress = t / 180.0f;

			cannon.RotateAround( cannon.position, cannon.up, slamVelocity );

			if( slamEvent != null )
				slamEvent.OnSlamUpdate( progress );
			//TODO scale needs to be world and not local. This gets the idea across for now though.
			//TODO maybe move the scale thing to a SlamBehaviour?
			connection.localScale = new Vector3(enemyScaling.Evaluate(progress), enemyScaling.Evaluate(progress), enemyScaling.Evaluate(progress));
			yield return null;
		}

		//Exit Slam - Return control to the enemy.


		Debug.LogWarning("Slam Done!", this);


		slamEvent.owner.Status = Situation.InControl;
		connection.parent = null;
		connection.localScale = Vector3.one;
		connection.rotation = Quaternion.identity; //TODO - find a better solution. This is here because enemies get "embedded" into the ground.

		if( slamEvent != null )
			slamEvent.OnSlamEnd();

		inSlam = false;

		ForceHeadRetract( connection.position ); //This also clears our connection variable. :)
	}

	public void ForceHeadRetract( Vector3 position )
	{
		head.gameObject.SetActive( true );
		connection = head.transform;
		head.ForceRetractFrom( position );
	}

	public void OnHeadHit( Transform t )
	{
		//When our head object hits a collider.
		connection = t;
		if( connection != null )
		{
			//Look for enemy script and tell it the situation.
			if( connection.GetComponent<Enemies.Enemy>() != null )
			{
				hookedEnemy = connection.GetComponent<Enemies.Enemy>();
				hookedEnemy.Status = Situation.Hooked;
			}
			connection.parent = null;
		}
		head.gameObject.SetActive(false);
	}

	public bool GetMouseHitSides( Vector2 mousePos )
	{
		BoxCollider2D[] walls = swipeDetector.GetComponents<BoxCollider2D>();
		if( walls[0].OverlapPoint(mousePos) || walls[1].OverlapPoint(mousePos) )
			return true;
		else
			return false;
	}

	////////////////// UTILITY FUNCTIONS /////////////

	//Notify the enemy of its situation.
	/*public static void SetEnemyControl( GameObject obj, bool state ) //True gives control, false removes it.
	{
		Enemies.Enemy enemy = obj.GetComponent<Enemies.Enemy>();

		if( state )
		{
			obj.transform.parent = null;
		}

		//if( enemy.HookComponent != null )
		//	enemy.HookComponent.Attach( !state );
		/*
		obj.layer = state ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("IgnorePhysics");



		/*
		obj.GetComponent<Rigidbody2D>().isKinematic = !state;


		else
		{
			//enemy.isI
		}
		/* Old method of doing this:
		enemy.GetComponent<Rigidbody2D>().isKinematic = !state;
		enemy.layer = state ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("IgnorePhysics");

		if( state == true ) 
			enemy.transform.parent = null; */
	//}









}
