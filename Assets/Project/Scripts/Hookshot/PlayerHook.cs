using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Enemies;
using Enemies.Modules;

//Stephan Ennen - 7/30/15

//NOTE: This script works very closely with PlayerHookHead.cs 
//      Not meant to be used standalone!

public class PlayerHook : MonoBehaviour 
{
	[Tooltip("Seperate hookhead script. This script works closely with it.")]
	public PlayerHookHead head;
	[Tooltip("We draw to our connection using this.")]
	public LineRenderer chain; 		//This is also what we draw our chain from.
	private Transform c; //Storage for the below accessor. (only use the below and ignore this)
	public Transform connection { //This is what we draw our chain to.
		get{ return c; }
		set{
			if( hookedEnemy != null )
				hookedEnemy.Status = Situation.InControl; //hookedEnemy.isHooked = false;
			c = value;
		}
	}
	[Tooltip("Our cannon head. We rotate this.")]
	public Transform cannon; 		//Our cannon head. We rotate this.
	[Tooltip("A visual to help the player know where the hook will travel.")]
	public Transform trajectoryVisual; 
	[Tooltip("A set of 2D colliders used to help detect mouse position relative to enemy and then react accordingly.")]
	public Transform swipeDetector; //Helps detect swipes.
	private CircleCollider2D detect_slam;
	private BoxCollider2D detect_clockwise;
	private BoxCollider2D detect_anticlockwise;

	[Tooltip("Overall speed our hook will fire or reel back in at.")]
	public float hookSpeed; 			  //speed our hook shoots out, or reels back in.
	[Tooltip("The hook will never go further from the player than this distance.")]
	public float maxHookDistance = 4f;  //Max distance our hook will travel.
	[Tooltip("Force multiplier for when we swing an enemy. Higher values mean faster travel.")]
	public float swingForce = 10.0f;    //How powerful each swing is.

	[Tooltip("How long it takes for the animation for slamming an enemy takes. (This isn't in seconds, rather a value that is added over time)")]
	public float slamVelocity = 10.0f;    //Essentially the animation Speed when slamming an enemy.
	[Tooltip("The enemy transform is scaled over time as they are tossed through the air.")]
	public AnimationCurve enemyScaling; //Scale multiplier of enemy as they are tossed.

	[Tooltip("Touch detection range around the player for starting to aim the hook.")]
	public float startHookAimTouchDistance = 1.25f; // Radius around the player in which a touch will start to aim a hook shot.
	[Tooltip("Touch detection range around the enemy for swinging or slamming it.")]
	public float hookedEnemyTouchDistance = 3.0f;   // Radius around a hooked enemy in which a touch will trigger (And then we can swing or slam the enemy depending on where the touch moves)
	[Tooltip("This swipe velocity must be exceeded in order to trigger a swing or a slam. (If not fast enough, nothing happens.)")]
	public float minSwipeVelocity = 0.025f; //Any swipe below this speed will not be detected.

	private Vector2 startDragPos = Vector2.zero;
	[Tooltip("Leave this value blank!")]
	public Enemies.Enemy hookedEnemy;

	void Start () 
	{
		if (head != null)
			head.gameObject.SetActive(false);
		if (trajectoryVisual != null)
			trajectoryVisual.gameObject.SetActive(false);

		detect_clockwise = swipeDetector.FindChild("Swipe_Clockwise").GetComponent<BoxCollider2D>();
		detect_anticlockwise = swipeDetector.FindChild("Swipe_Anti-Clockwise").GetComponent<BoxCollider2D>();
		detect_slam = swipeDetector.FindChild("Swipe_Slam").GetComponent<CircleCollider2D>();
	}

	private Vector2 _lastMousePos = Vector2.zero;
	private float mouseSpeed;

	void Update() 
	{

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
		mouseSpeed = Vector2.Distance( _lastMousePos, VectorExtras.GetMouseWorldPos() ) * Time.deltaTime;

		if (connection == null) // Our head is not present and there is no enemy attached. (Do code for a new head)
		{
			if (startDragPos == Vector2.zero)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Vector2 mPos = VectorExtras.GetMouseWorldPos();
					if (mPos.magnitude < startHookAimTouchDistance)
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
						if (Vector2.Distance(mPos, VectorExtras.V2FromV3(connection.position)) < hookedEnemyTouchDistance) //Make sure we're starting the click somewhat close to our hooked enemy. TODO make public var
						{
							startDragPos = mPos;

							swipeDetector.position = connection.position; //Set our triggers to the enemy location, and make it face toward the tower.
							Vector2 direction = VectorExtras.Direction( VectorExtras.V2FromV3(connection.position), VectorExtras.V2FromV3(transform.position) );
							TransformExtensions.SetRotation2D( swipeDetector, VectorExtras.VectorToDegrees(direction) );

						}
					}
				}
				else
				{
					Vector2 mPos = VectorExtras.GetMouseWorldPos();
					if( Input.GetMouseButton(0) ) //Player is holding..
					{
						if( mouseSpeed > minSwipeVelocity )
						{
							if( detect_clockwise.OverlapPoint( mPos ) == true )
							{
								Vector3 dir = swipeDetector.up;                                      
								hookedEnemy.SwingComponent.NotifySwing( true, dir );				 //dir * force * slamAOE Diameter
								connection.GetComponent<Rigidbody2D>().AddForce( new Vector2(dir.x, dir.y) * swingForce * (1.3f * 2.0f), ForceMode2D.Force );
								startDragPos = Vector2.zero;
							}
							else if( detect_anticlockwise.OverlapPoint( mPos ) == true )
							{
								Vector3 dir = -swipeDetector.up;
								hookedEnemy.SwingComponent.NotifySwing( false, dir );
								connection.GetComponent<Rigidbody2D>().AddForce( new Vector2(dir.x, dir.y) * swingForce * (1.3f * 2.0f), ForceMode2D.Force );
								startDragPos = Vector2.zero;
							}
							else if( detect_slam.OverlapPoint( mPos ) == true )
							{
								StartCoroutine( Slam() );
								startDragPos = Vector2.zero;
							}
						}


						//OLD way
						//connection.transform.position = VectorExtras.OffsetPosInPointDirection(transform.position, smoothedDragPos, Vector3.Distance(transform.position, connection.position)); 
					}
					else if (Input.GetMouseButtonUp(0)) //Player let go.
					{
						//Apply our swing physics here if we want it.

						startDragPos = Vector2.zero;
					}
				}
			}
		}

		_lastMousePos = VectorExtras.GetMouseWorldPos();
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
