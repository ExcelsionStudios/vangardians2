using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//Stephan Ennen - 6/18/15

[RequireComponent(typeof(LineRenderer))]
public class SimpleHook : MonoBehaviour 
{
	public SimpleHookHead head;
	public LineRenderer chain; 		//This is also what we draw our chain from.
	public Transform connection; 	//This is what we draw our chain to.
	public Transform cannon; 		//Our cannon head. We rotate this.

	public float speed; 			      //Speed our hook shoots out.
	public float slamVelocity = 10.0f;    //Essentially the animation speed when slamming an enemy.
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
						head.GetComponent<Rigidbody2D>().Sleep();
						//TODO incorporate some kind of manual code to ensure the enemy always gets pushed to the position opposite of our mouse.
						head.transform.position = VectorExtras.V3FromV2( -VectorExtras.OffsetPosInPointDirection( VectorExtras.V2FromV3(transform.position), mPos, 0.61f ), 0.0f );
						head.GetComponent<Rigidbody2D>().AddForce( VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), VectorExtras.V2FromV3(head.transform.position)) * speed);
						head.distanceTimeout = Vector3.Distance( VectorExtras.V3FromV2( mPos, 0 ), this.transform.position );

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

				/* //I dont think this control will need a joint. (We will control position manually.)
				connection.GetComponent<Rigidbody2D>().isKinematic = false;
				DistanceJoint2D dj = connection.GetComponent<DistanceJoint2D>();
				if( dj == null )
					dj = connection.gameObject.AddComponent<DistanceJoint2D>();
				
				dj.connectedAnchor = VectorExtras.V2FromV3( transform.position );
				dj.distance = Vector3.Distance( transform.position, connection.position );
				*/

				//This is an array of positions of the mouse. This next bit is gonna be complicated.
				
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
						path = ArrayTools.PushLast<Vector2>(path, mPos);
						
						//We calculate the angle from last frame's position to this frame's position, then add it to totalAngle.
						Vector2 prev = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), lastPos); //This will cause problems if the player moves!
						Vector2 curr = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), mPos);
						float deltaAng = Vector2.Angle( prev, curr );
						totalAngle += deltaAng;


						//Look to see if our detlta angle was of a certian size. 
						//If large enough, we can assume that the path was a slam. Forcefully end the touch logic if this is the case.
						//TODO This detection is vulnerable to lag!!!!
						if( deltaAng > slamSensitivity )
						{
							StartCoroutine( Slam() );
							startDragPos = Vector2.zero; //Setting this exits logic.
							return;
						}


						//Move our enemy.
						Vector3 mousePosV3 = VectorExtras.V3FromV2( mPos, 0f );
						connection.transform.position = VectorExtras.OffsetPosInPointDirection(transform.position, mousePosV3, Vector3.Distance(transform.position, connection.position)); //TODO track touch offset at beginning of touch.


						
					}
					else if (Input.GetMouseButtonUp(0)) //Player let go.
					{
						path = ArrayTools.PushLast<Vector2>(path, mPos);
						Vector2 playerDir = VectorExtras.Direction( VectorExtras.V2FromV3(connection.position), VectorExtras.V2FromV3(this.transform.position) );


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
		inSlam = true;
		connection.parent = cannon;
		//TODO disable physics on the connection until this is done, disable enemy movement code.
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
		connection.parent = null;
		connection.localScale = Vector3.one;
		connection = null;
		inSlam = false;
	}

	public void OnHeadHit( Transform t )
	{
		//When our head object hits a collider.
		connection = t;
		head.GetComponent<Rigidbody2D>().Sleep();
		head.gameObject.SetActive(false);
	}




}
