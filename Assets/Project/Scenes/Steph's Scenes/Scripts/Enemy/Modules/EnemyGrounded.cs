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
		[Tooltip("This prefab should look identical to this enemy. It should also have 3D rigidbody and collider(s) attached. (so it falls off of our cliff)")]
		public GameObject corpse;

		internal override void AlwaysUpdate()
		{
			if( owner.Status != Situation.BeingSlammed )
			{
				if( Physics.Raycast( transform.position, Vector3.forward, 0.1f ) == false ) //out data,
				{
					Fall();
				}
			}
		}

		void Fall()
		{
			if( owner.Status == Situation.Hooked ) //Tell our hook to retract.
			{
				GameObject obj = GameObject.Find("Player") as GameObject; //TODO Find methods are VERY slow. Use an alternate route.
				PlayerHook hook = obj.GetComponent<PlayerHook>();
				hook.ForceHeadRetract( transform.position );
			}

			Rigidbody2D r2D = GetComponent<Rigidbody2D>();
			Vector2 velocity = r2D.velocity;

			GameObject body = (GameObject)Instantiate( corpse, this.transform.position, this.transform.rotation );
			body.GetComponent<Rigidbody>().velocity = new Vector3( velocity.x, velocity.y, 0.0f ); //Maintain velocity for seamless transition.
			//body.GetComponent<Rigidbody>().angularVelocity TODO apply ang velocity.


			GameObject.Destroy( this.gameObject ); //Dont use kill here - we dont want other modules making blood or anything else, it doesnt makesense when falling.
		}
	}
}