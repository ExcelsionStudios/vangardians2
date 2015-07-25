using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen 7/24/15

//Simple class that checks to see if enemy has fallen off of a cliff, 
// and if so, switch to 3D physics, simulate gravity and kill this unit after a bit.

namespace Enemies.Modules
{
	public class EnemyGrounded : ModuleBase
	{
		public GameObject corpse;

		void FixedUpdate()
		{
			if( owner.hasFallen == false && owner.isHooked == false )
			{
				if( Physics.Raycast( transform.position, Vector3.forward, 0.1f ) == false )
				{
					Fall();
				}
			}
		}

		void Fall()
		{

			Rigidbody2D r2D = GetComponent<Rigidbody2D>();
			Vector2 velocity = r2D.velocity;

			GameObject body = (GameObject)Instantiate( corpse, this.transform.position, this.transform.rotation );
			body.GetComponent<Rigidbody>().velocity = new Vector3( velocity.x, velocity.y, 0.0f ); //Maintain velocity for seamless transition.
			//body.GetComponent<Rigidbody>().angularVelocity TODO apply ang velocity.


			GameObject.Destroy( this.gameObject ); //Dont use kill here - we dont want other modules making blood or anything else, it doesnt makesense when falling.



			/*
			foreach( ModuleBase module in owner.modules )
			{
				MoveBehaviour moveModule = module as MoveBehaviour;
				if( moveModule != null )
					GameObject.DestroyImmediate( moveModule );
			}

			GameObject.DestroyImmediate( r2D ); //DestroyImmediate is required here otherwise it wont let us add the 3D system
			float radius = GetComponent<CircleCollider2D>().radius;
			GameObject.DestroyImmediate( GetComponent<Collider2D>() );

			SphereCollider c = this.gameObject.AddComponent<SphereCollider>() as SphereCollider;
			c.radius = radius;
			Rigidbody r = this.gameObject.AddComponent<Rigidbody>() as Rigidbody;
			r.velocity = new Vector3( velocity.x, velocity.y, 0.0f ); //Maintain velocity for seamless transition.

			owner.hasFallen = true;

			StartCoroutine( "DestroyDelay" ); */



		}
		/*
		IEnumerable DestroyDelay()
		{
			yield return new WaitForSeconds( 3.0f );
			owner.Kill();
		} */
	}
}