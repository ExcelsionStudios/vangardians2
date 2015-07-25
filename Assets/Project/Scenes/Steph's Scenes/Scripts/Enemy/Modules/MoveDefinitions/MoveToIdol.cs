using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen 7/24/15

//Tries to move to the idol. (Until we get an idol, this just moves to the player) Using FixedMoveUpdate or MoveUpdate will ensure that the enemy is mobile when moving.
namespace Enemies.Modules
{
	//[RequireComponent(typeof(Rigidbody2D))]
	public class MoveToIdol : MoveBehaviour
	{
		public float force;
		public Transform idol;
		private Rigidbody2D rb2D;
		void Start()
		{
			rb2D = GetComponent<Rigidbody2D>();
		}

		internal override void FixedMoveUpdate()
		{
			if( rb2D != null )
			{
				Vector2 targetDir = VectorExtras.Direction( VectorExtras.V2FromV3(this.transform.position), VectorExtras.V2FromV3(idol.position));
				rb2D.AddForce( targetDir * force, ForceMode2D.Force );

				TransformExtensions.SetRotation2D(this.transform, VectorExtras.VectorToDegrees(rb2D.velocity.normalized));
			}

		}


	}
}