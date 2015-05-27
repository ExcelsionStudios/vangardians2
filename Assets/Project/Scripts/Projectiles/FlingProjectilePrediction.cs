using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Stephen - Around 5/17/2015. Some commenting and additions by Matt McGrath.

// Primary class for both predicting where a launched projectile will land and launching the projectile itself.
// Matt's TODO Possible Suggestion (If Possible): Make this work with all masses of projectile, if designer calls for heavier or lighter projectiles that might require different pull-back lengths for aiming.
[RequireComponent(typeof(LineRenderer))]
public class FlingProjectilePrediction : MonoBehaviour 
{
	public GameObject projectilePrefab;			// Reference to projectile we will use. We change this in the ChangeProjectile script via a scroll list of projectile choices.
	public GameObject impactMarker;				// Reference to our "landing zone," basically. It will be displayed on the terrain where the projectile will land.
	public float projectileMass = 1f; 			// Used during prediction. Wrong values means wrong prediction!
	public float throwForce = 360f;
	public float mouseDragSensitivity = 380f; 	// Lower values are more sensitive.
	public float extraHeight = 280f;

	private LineRenderer line;
	public int vertCount = 128;

	private Vector2 mouseStartDragPos; 			// Our mouse pos when we started a throw.

	// For now, let's reference our PlayerTower (lazily thru inspector without safety checks =D). In future, a better design choice might be that the PlayerTower references THIS script, 
	// as the tower is technically the object that possess this ability.
	public PlayerTower playerTower;	

	// On start, cache the LineRenderer component, set its vertex count, and set the impact maker to inactive.
	void Start() 
	{
		line = GetComponent<LineRenderer>();
		if (line == null)
			Debug.LogError("No LineRenderer component is attached!", this);
		else
			line.SetVertexCount(0);

		if ( impactMarker == null)
			Debug.LogError("No ImpactMarker gameobject is specified!", this);
		else
			impactMarker.SetActive(false);
	}

	// If weren't not clicking over a UI element, do the required throw logic.
	void Update()
	{
		// Use this line to prevent predicting or launching when we hit a Button or UI object via a mouse click. TODO: Seems to no longer apply with new radial menu.
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			DoThrowLogic();
		}

		//Esai Solorio Changes
		//This sets the color of the line. If u have mana it will set to red, otherwise it will be gray
		if (projectilePrefab.GetComponent<Projectile>().ManaCost <= playerTower.Mana)
			line.material.color = Color.red;
		else
			line.material.color = Color.gray;
	}

	void DoThrowLogic()
	{
		if (Input.GetButtonDown("Fire1")) 		// Player has started a throw.
		{
			impactMarker.SetActive(true);
			mouseStartDragPos = Input.mousePosition;
		}

		if (Input.GetButton("Fire1")) 			// Player is aiming. Draw a predicted path.
		{
			Vector3 predictedForce = GetThrowForce();
			DrawPrediction(transform.position, predictedForce / (50.0f * projectileMass)); //50 seems to be the best value for accurate prediction. Modify with care!
			// From Matt: What is this 50.0f magic number? =o lol. Explains why we're stuck with singular-massed objects perhaps?
		}
		else if (Input.GetButtonUp("Fire1")) 	// Player has let go. Create a projectile and fire it.
		{
			Debug.Log("Our Mana: " + playerTower.Mana + ", Projectile's Cost: " + projectilePrefab.GetComponent<Projectile>().ManaCost);

			// Matt - 5/22/2015: With Mana System, we want new checks for if we should create the projectile. Do we have the Mana required to launch this projectile?
			Projectile projectile = projectilePrefab.GetComponent<Projectile>();
			float manaCost = projectile.ManaCost;
			if (manaCost <= playerTower.Mana)
			{
				// Drain the player's Mana by the appropriate amount.
				playerTower.Mana -= manaCost;

				// Create the appropriate projectile by Instantiating its prefab.
				GameObject newProjectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, projectilePrefab.transform.rotation);


				// Begin new Changes 5/27, Matt
				Projectile newProj = newProjectile.GetComponent<Projectile>();
				if (newProj.arcHeight != 0 && newProj.Speed != 0)				// Change later once each Projectile Prefab has these values defined.
				{
					extraHeight = newProj.arcHeight;
					throwForce = newProj.Speed;
				}

				// End new changes. Remove these lines if anything odd begins to happen.

				newProjectile.GetComponent<Rigidbody>().AddForce( GetThrowForce() );
				
				line.SetVertexCount(0);
				impactMarker.SetActive(false);
				mouseStartDragPos = Vector2.zero;
			}

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
		Debug.Log("Extra Height: " + extraHeight + ", Throw Force: " + throwForce);
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
}
