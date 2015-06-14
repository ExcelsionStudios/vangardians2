using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour 
{
	public HookHead head;
	public LineRenderer chain; //This is also what we draw our chain from.
	public Transform connection; //This is what we draw our chain to.
	public Transform cannon; //Our cannon head. We rotate this.
	public float speed; //Speed our hook shoots out.
	public float tanSpeed; //Tangental swing speed for swinging enemies around.

	private Vector2 startDragPos = Vector2.zero;
	public Vector2[] path = new Vector2[0]; 
	float totalAngle = 0f;
	
	void Start () 
	{
		head.gameObject.SetActive(false);
	}
	

	void Update () 
	{
		ReadInput ();

		//Update our line renderer.
		if( connection != null )
		{
			chain.SetPosition(1, chain.transform.position); //This order makes it appear that chain comes out of the cannon.
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
		//#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if( connection == null ) //Our head is not present and there is no enemy attached. (Do code for a new head)
		{
			if( startDragPos == Vector2.zero )
			{
				if( Input.GetMouseButtonDown(0) )
				{
					Vector2 mPos = VectorExtras.GetMouseWorldPos();
					if( mPos.magnitude < 1.25f )
					{
						startDragPos = mPos;
					}
				}
			}
			else
			{

				if( Input.GetMouseButton(0) ) //Player is aiming
				{
					Vector2 direction = -VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), VectorExtras.GetMouseWorldPos() );
					TransformExtensions.SetRotation2D( cannon, VectorExtras.VectorToDegrees(direction) );
				}
				else if( Input.GetMouseButtonUp(0) ) //Player let go.
				{
					Vector2 mPos = VectorExtras.GetMouseWorldPos();

					head.gameObject.SetActive(true);
					head.GetComponent<Rigidbody2D>().Sleep();
					head.transform.position = VectorExtras.V3FromV2( -VectorExtras.OffsetPosInPointDirection( VectorExtras.V2FromV3(transform.position), mPos, 0.61f ), 0.0f );
					head.GetComponent<Rigidbody2D>().AddForce( VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), VectorExtras.V2FromV3(head.transform.position)) * speed);
					connection = head.transform;

					startDragPos = Vector2.zero;
				}
			}
		}
		else //Either an enemy is attached or a head.
		{
			//Always look at our connection.
			//Vector2 direction = -VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), VectorExtras.V2FromV3(connection.position) );
			TransformExtensions.SetRotation2D( cannon, VectorExtras.VectorToDegrees(VectorExtras.Direction( VectorExtras.V2FromV3(transform.position), VectorExtras.V2FromV3(connection.position) )) );
			
			
			if( head.isActiveAndEnabled == true )
			{
				//Do nothing (for now)
			}
			else
			{
				//The head is not present. We must have an enemy.
				connection.GetComponent<Rigidbody2D>().isKinematic = false;
				DistanceJoint2D dj = connection.GetComponent<DistanceJoint2D>();
				if( dj == null )
					dj = connection.gameObject.AddComponent<DistanceJoint2D>();
				
				dj.connectedAnchor = VectorExtras.V2FromV3( transform.position );
				dj.distance = Vector3.Distance( transform.position, connection.position );



				//This is an array of positions of the mouse. This next bit is gonna be complicated.



				if( startDragPos == Vector2.zero )
				{
					if( Input.GetMouseButtonDown(0) )
					{
						Vector2 mPos = VectorExtras.GetMouseWorldPos();
						if( Vector2.Distance(mPos, VectorExtras.V2FromV3(connection.position)) < 3.0f ) //Make sure we're starting the click somewhat close to our hooked enemy.
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
						Vector2 lastPos = path[path.Length-1];
						path = ArrayTools.PushLast<Vector2>(path, mPos);
						
						//We calculate the angle from last frame's position to this frame's position, then add it to totalAngle.
						Vector2 prev = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), lastPos); //This will cause problems if the player moves!
						Vector2 curr = VectorExtras.Direction(VectorExtras.V2FromV3(transform.position), mPos);
						//float constVal = Mathf.Min( 1.0f, Dot(prev, curr) / (prev.magnitude * curr.magnitude) );
						float deltaAng = Vector2.Angle( prev, curr );
						//Debug.Log( totalAngle + " + " + deltaAng );
						totalAngle += deltaAng;

					}
					else if( Input.GetMouseButtonUp(0) ) //Player let go.
					{
						path = ArrayTools.PushLast<Vector2>(path, mPos);
						Vector2 playerDir = VectorExtras.Direction( VectorExtras.V2FromV3(connection.position), VectorExtras.V2FromV3(this.transform.position) );

						//direction from our starting pos to our end pos.
						Vector2 swipeDir = VectorExtras.Direction( startDragPos, mPos ); 
						
						float generalDot = Vector2.Dot( swipeDir, playerDir ); //Look up Vector3.Dot on unity wiki for what this does.
						//Test to see if the direction from start to finish is close to the direction to the player.
						if( generalDot > 0.8f )
						{ 
							//The directions were simmilar, but this doesnt mean that this is a valid "fling". The player could have made a half-circle.
							//Test the variance in direction in the actual path.
							for( int i = 1; i < path.Length; i++ )
							{
								//Test if dots are close to zero.

								float subDot = Vector2.Dot( swipeDir, VectorExtras.Direction( path[i - 1], path[i] ) );
								Debug.Log( subDot );
								if( subDot < 0.4f )
								{
									// Add exit function here, then return.
									Debug.Log( "Direction variance was too large! (This is a swing)" );
									//TODO make swipeDir snap to exactly tangental.
									connection.GetComponent<Rigidbody2D>().AddForce(swipeDir * tanSpeed * (totalAngle / 36f));
									//DONT let go of the enemy yet. (Maybe if its dead?)




									break;
								}
								if( i == path.Length - 1 )
								{
									Debug.Log( "Direction straight as an arrow! (This is a swipe)" );
									connection.GetComponent<Rigidbody2D>().isKinematic = true; //Disabling physics would be better....
									connection = null;
								}
							}


						}
						else
						{
							//Apply swing forces.

							connection.GetComponent<Rigidbody2D>().AddForce(swipeDir * tanSpeed * (totalAngle / 36f));

						}

						startDragPos = Vector2.zero;

					}
				}

			}

		}




		/*
					else if( Input.GetMouseButtonUp(0) ) //Player let go.
					{

						
						Vector2 swipeDir = VectorExtras.Direction( startDragPos, mPos );
						Vector2 playerDir = VectorExtras.Direction( VectorExtras.V2FromV3(connection.position), VectorExtras.V2FromV3(this.transform.position) );

						//////
						//Dot doesnt seem to be that user friendly. 
						//Maybe we should keep an array of the general positions of the inputs, somehow detect circle from line?
						//////

						float dot = Vector2.Dot( swipeDir, playerDir ); //Look up Vector3.Dot on unity wiki for what this does.
						Debug.Log( dot );
						if( Mathf.Abs( dot ) < 0.6f ) //Swiping tangental to our player.
						{
							Debug.Log("Tangental swipe");
							//TODO make swipeDir snap to exactly tangental.
							connection.GetComponent<Rigidbody2D>().AddForce(swipeDir * tanSpeed);
							//DONT let go of the enemy yet. (Maybe if its dead?)
						}
						else if( dot >= 0.6f ) //We leave out the Mathf.abs because we don't care if we swipe away from our player.
						{
							Debug.Log("Player swipe");
							connection.GetComponent<Rigidbody2D>().isKinematic = true; //Disabling physics would be better....
							connection = null;
							startDragPos = Vector2.zero;
						}
						else
						{

							Debug.Log("Offscreen swipe..."); //Uhhhhhhhh do something here
							connection.GetComponent<Rigidbody2D>().isKinematic = true;
							connection = null;
							startDragPos = Vector2.zero;
						}
					}
					*/




		//#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		//TODO - mobile control.

		//#endif

	}
	public void OnHeadHit( Transform t )
	{
		//When our head object hits a collider.
		connection = t;
		head.GetComponent<Rigidbody2D>().Sleep();
		head.gameObject.SetActive(false);
		//startDragPos = Vector2.zero;
	}


	/*

	void Awake () 
	{
		chain = GetComponent<LineRenderer>();
		chain.material.mainTextureScale = new Vector2( 1,1 );
	}
	
	//CANT read linerenderer positions. We must store and read from our own array of positions, as we set them.

	
	float GetRenderLength()
	{
		return 0f;//lineRenderer.Set
	} */






}
