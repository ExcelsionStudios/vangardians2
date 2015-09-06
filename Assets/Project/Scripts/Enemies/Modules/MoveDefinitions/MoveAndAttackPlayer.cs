using System.Collections;
using UnityEngine;
using Enemies;
using Utils.Audio;

// Matt McGrath - 9/06/2015

namespace Enemies.Modules
{
	//[RequireComponent(typeof(Rigidbody2D))]
	// Movement Module which allows behavior for seeking the Player, traveling to him, then attacking.
	public class MoveAndAttackPlayer : MoveBehaviour
	{
		#region Fields

		public float force;
		private Transform player;
		private Rigidbody2D rb2D;
		public AudioClip[] playerAttackedSounds;	// Set in Editor for now.
		private Vector3 playerPosition;
		private bool isAttacking;

		#endregion

		void Start()
		{
			rb2D = GetComponent<Rigidbody2D>();

			// TODO: Player class? Then get the component and location of him here. But for now, he's static.
			//player = Player.Get().transform;
			playerPosition = Vector3.zero;
		}

		#region Updates

		internal override void FixedMoveUpdate()
		{
			if (rb2D != null)
			{
				MoveToPlayer();
			}
			
		}

		#endregion
		
		void MoveToPlayer()
		{
			MoveToPosition(playerPosition);
		}

//		void OnCollisionEnter2D(Collision2D col)
//		{
//			if (owner.isImmobile == false)
//			{
//				if (col.transform.tag == "Player")
//				{
//					isAttacking = true;
//					// Play a sound to indicate Player was attacked.
//					if (playerAttackedSounds != null && playerAttackedSounds.Length > 0)
//						AudioHelper.PlayClipAtPoint(playerAttackedSounds[Random.Range(0, playerAttackedSounds.Length)], transform.position);
//				}
//
//				// TODO: Reduce player health at intervals, or one-hit KO him?
//			}
//		}

		// This method will always run, regardless of if this component is disabled or not.
		internal override void AlwaysUpdate()
		{
			base.AlwaysUpdate();
			
			if (owner.isImmobile)
			{

			}
		}

		void OnDestroy()
		{
		}

		#region Move To Positions

		// Moves to the specified Transform.
		void MoveToPosition(Transform pos)
		{
			MoveToPosition(pos.position);
		}

		// Moves to the specified 3D vector. Internally converts thsi to a 2D vector due to our Physics setup.
		void MoveToPosition(Vector3 pos)
		{
			MoveToPosition(new Vector2(pos.x, pos.y));
		}

		// Moves to the specified 2D vector. Called from the 3D vector version of this, which is called by Trasnform version.
		void MoveToPosition(Vector2 pos)
		{
			Vector2 targetDir = VectorExtras.Direction(VectorExtras.V2FromV3(this.transform.position), pos);
			rb2D.AddForce(targetDir * force, ForceMode2D.Force);
			
			TransformExtensions.SetRotation2D(this.transform, VectorExtras.VectorToDegrees(rb2D.velocity.normalized));
		}

		#endregion
	}
}