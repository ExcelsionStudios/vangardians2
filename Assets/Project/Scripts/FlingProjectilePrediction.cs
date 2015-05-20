using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class FlingProjectilePrediction : MonoBehaviour 
{
	public GameObject projectilePrefab;
	public GameObject impactMarker;
	public float projectileMass = 1f; //Used during prediction. Wrong values means wrong prediction!
	public float throwForce = 360f;
	public float mouseDragSensitivity = 380f; //Lower values are more sensitive.
	public float extraHeight = 280f;

	private LineRenderer line;
	public int vertCount = 128;

	private Vector2 mouseStartDragPos; //Our mouse pos when we started a throw.
	void Start () 
	{
		line = GetComponent<LineRenderer>();
		if( line == null )
			Debug.LogError("No LineRenderer component is attached!", this);
		else
			line.SetVertexCount( 0 );

		if( impactMarker == null )
			Debug.LogError("No ImpactMarker gameobject is specified!", this);
		else
			impactMarker.SetActive( false );
	}


	void Update()
	{
		DoThrowLogic();
	}

	void DoThrowLogic()
	{
		if( Input.GetButtonDown("Fire1") ) //Player has started a throw.
		{
			impactMarker.SetActive( true );
			mouseStartDragPos = Input.mousePosition;
		}

		if( Input.GetButton("Fire1") ) //Player is aiming. Draw a predicted path.
		{
			Vector3 predictedForce = GetThrowForce();
			DrawPrediction(transform.position, predictedForce / (50.0f * projectileMass)); //50 seems to be the best value for accurate prediction. Modify with care!
		}
		else if( Input.GetButtonUp("Fire1") ) //Player has let go. Create a projectile and fire it.
		{
			GameObject newProjectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, projectilePrefab.transform.rotation);
			newProjectile.GetComponent<Rigidbody>().AddForce( GetThrowForce() );

			line.SetVertexCount( 0 );
			impactMarker.SetActive( false );
			mouseStartDragPos = Vector2.zero;
		}
	}

	Vector2 GetThrowDirection()
	{
		return VectorExtras.Direction((Vector2)Input.mousePosition, (Vector2)mouseStartDragPos) * //This first bit gets the base direction. We must cast as Vector2 due to conflicts with Vector3.
			 ( VectorExtras.ReverseLerp(Vector2.Distance(Input.mousePosition, mouseStartDragPos), 0.0f, mouseDragSensitivity) * throwForce );
	}
	Vector3 GetThrowForce()
	{
		Vector2 throwDir = GetThrowDirection();
		return new Vector3(throwDir.x, extraHeight, throwDir.y);
	}


	void DrawPrediction( Vector3 startPos, Vector3 startVelocity )
	{
		line.SetVertexCount( vertCount );

		Vector3 curPos = startPos;
		Vector3 curVel = startVelocity;
		for(int i = 0; i < vertCount; i++)
		{
			line.SetPosition(i, curPos);

			if( curPos.y < 0.0f ) //TODO - add raycast instead of checking y. This currently wont work with terrain or varied elevation.
			{
				impactMarker.transform.position = new Vector3( curPos.x, 0.01f, curPos.z );
				//Only do three additional loops after this. (This is so that the line renderer always goes into the ground)
				line.SetVertexCount( i + 3 );
				for( int j = 0; j < 3; j++ )
				{
					curVel += Physics.gravity * Time.fixedDeltaTime;
					curPos += curVel * Time.fixedDeltaTime;
					line.SetPosition(i + j, curPos);
				}
				break;
			}

			curVel += Physics.gravity * Time.fixedDeltaTime;
			curPos += curVel * Time.fixedDeltaTime;
		}
	}













	/*
	public float upwardForce;
	public float sideForce;
	public Vector2 lastMousePosition;
	public Vector2 direction;
	public float maxForce;
	Vector3 GetForce()
	{
		direction = new Vector2 (Mathf.Clamp(Input.mousePosition.x - lastMousePosition.x, -maxForce, maxForce) / maxForce,
		                         Mathf.Clamp(Input.mousePosition.y - lastMousePosition.y, -maxForce, maxForce) / maxForce );
		return new Vector3(-direction.x * sideForce, upwardForce, -direction.y * sideForce) ;
	}
	
	void Update () 
	{
		if( Input.GetButton("Fire1") ) 
		{
			//lastMousePosition = Input.mousePosition;
			DrawPrediction( transform.position, GetForce() );
		}
		else
		{
			line.SetVertexCount( 0 );
		}

		if( Input.GetButtonUp("Fire1") ) 
		{
			direction = new Vector2 (Mathf.Clamp(Input.mousePosition.x - lastMousePosition.x, -maxForce, maxForce) / maxForce,
			                         Mathf.Clamp(Input.mousePosition.y - lastMousePosition.y, -maxForce, maxForce) / maxForce );
			GameObject newProjectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, projectilePrefab.transform.rotation);
			newProjectile.GetComponent<Rigidbody>().AddForce(Vector3.up * upwardForce);
			newProjectile.GetComponent<Rigidbody>().AddForce(new Vector3(-direction.x, 0, -direction.y) * sideForce);
		}
		lastMousePosition = Input.mousePosition;
	}
	*/

}
