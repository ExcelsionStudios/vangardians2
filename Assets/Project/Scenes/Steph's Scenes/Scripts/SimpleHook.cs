using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//Stephan Ennen - 6/24/15

//NOTE: This script works very closely with SimpleHookHead.cs 
//      Not meant to be used standalone!

[RequireComponent(typeof(LineRenderer))]
public class SimpleHook : MonoBehaviour 
{
	public SimpleHookHead head;
	public LineRenderer chain; 		//This is also what we draw our chain from.
	public Transform connection; 	//This is what we draw our chain to.
	public Transform cannon; 		//Our cannon head. We rotate this.

	public float hookSpeed; 			  //speed our hook shoots out, or reels back in.
	public float maxHookDistance = 0.8f;  //Max distance our hook will travel.
	public float slamVelocity = 10.0f;    //Essentially the animation Speed when slamming an enemy.
	public float slamSensitivity = 90.0f; //If our delta angle is larger than this while dragging, it triggers a slam.

	public AnimationCurve enemyScaling; //Scale multiplier of enemy as they are tossed.

	private Vector2 startDragPos = Vector2.zero;
	public Vector2[] path = new Vector2[0]; 
	float totalAngle = 0f;

	void Start () 
	{
		if (head != null)
			head.gameObject.SetActive(false);
	}

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
				
				if (Input.GetMouseButton(0)) // Player is aiming
				{
					Vector2 direction = -VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), VectorExtras.GetMouseWorldPos() );
					TransformExtensions.SetRotation2D( cannon, VectorExtras.VectorToDegrees(direction) );
				}
				else if (Input.GetMouseButtonUp(0)) // Player let go.
				{
					Vector2 mPos = VectorExtras.GetMouseWorldPos();
					
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
				if (startDragPos == Vector2.zero)
				{
					if (Input.GetMouseButtonDown(0))
					{
						Vector2 mPos = VectorExtras.GetMouseWorldPos();
						if (Vector2.Distance(mPos, VectorExtras.V2FromV3(connection.position)) < 3.0f) //Make sure we're starting the click somewhat close to our hooked enemy.
						{
							startDragPos = mPos;

							path = new Vector2[0];
							path = ArrayTools.PushLast<Vector2>(path, mPos);

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
						
						//We calculate the angle from last frame's position to this frame's position, then add it to totalAngle.
						Vector2 prev = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), lastPos); //This will cause problems if the player moves!
						Vector2 curr = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), mPos);
						float deltaAng = Vector2.Angle( prev, curr );
						totalAngle += deltaAng;


						//Look to see if our detlta angle was of a certian size. 
						//If large enough, we can assume that the path was a slam. Forcefully end the touch logic if this is the case.
						//TODO This detection is vulnerable to lag!!!! (Add some sort of time.deltatime)
						//TODO make this detection more accurate.
						if( deltaAng > slamSensitivity )
						{
							StartCoroutine( Slam() );
							return;
						}


						//Move our enemy.
						Vector3 mousePosV3 = VectorExtras.V3FromV2( mPos, 0f );
						connection.transform.position = VectorExtras.OffsetPosInPointDirection(transform.position, mousePosV3, Vector3.Distance(transform.position, connection.position)); //TODO track touch offset at beginning of touch.

					}
					else if (Input.GetMouseButtonUp(0)) //Player let go.
					{
						path = ArrayTools.PushLast<Vector2>(path, mPos);


						//Apply our swing physics here if we want it.



						
						startDragPos = Vector2.zero;
					}
				}
			}
		}
	}
	public float debugT = 0f;
	private bool inSlam = false;
	IEnumerator Slam() //Animate our slam, then do damage to enemies in radius.
	{
		startDragPos = Vector2.zero;
		inSlam = true;
		connection.parent = cannon;

		SetEnemyControl(connection.gameObject, false);

		float t = 0f;
		while( t < 180f )
		{
			//TODO make this not able to overshoot, add Time.deltaTime multiplier
			t += slamVelocity;
			//cannon.rotation = Quaternion.Slerp(startRot, targetRotation, debugT);
			cannon.RotateAround( cannon.position, cannon.up, slamVelocity );
			//TODO scale needs to be world and not local. This gets the idea across for now though.
			connection.localScale = new Vector3(enemyScaling.Evaluate(t / 180f), enemyScaling.Evaluate(t / 180f), enemyScaling.Evaluate(t / 180f));
			yield return new WaitForEndOfFrame();
		}
		//Exit

		Debug.LogWarning("Slam Done");
		SetEnemyControl(connection.gameObject, true);
		connection.localScale = Vector3.one;

		DoSlamEffects();

		connection = null;
		inSlam = false;
	}
	void DoSlamEffects() //Deal damage to connection (enemy) or whatever you want to here.
	{

	}

	public void OnHeadHit( Transform t )
	{
		//When our head object hits a collider.
		connection = t;

		//TODO we may want to set enemy control to false here.

		head.gameObject.SetActive(false);
	}

	////////////////// UTILITY FUNCTIONS /////////////

	//This function assumes a lot of things about the enemy gameobject, at least until we know what our enemies will look like.
	public static void SetEnemyControl( GameObject enemy, bool state ) //True gives control, false removes it.
	{
		enemy.GetComponent<Rigidbody2D>().isKinematic = !state;
		enemy.layer = state ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("IgnorePhysics");

		if( state == true ) 
			enemy.transform.parent = null;
	}









}
