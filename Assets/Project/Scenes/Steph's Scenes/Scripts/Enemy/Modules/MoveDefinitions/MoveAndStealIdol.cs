using UnityEngine;
using System.Collections;
using Enemies;
using Utils.Audio;

//Stephan Ennen 7/27/15

//Tries to move to the idol, then tries to run away with it.

namespace Enemies.Modules
{
	//[RequireComponent(typeof(Rigidbody2D))]
	public class MoveAndStealIdol : MoveBehaviour
	{
		public float force;
		public Transform holdingPosition;
		private Transform idol;
		private Rigidbody2D rb2D;
		public bool HasIdol{
			get{ return holdingPosition.childCount > 0; }
		}

		public AudioClip[] idolGrabbedSounds;	// Set in Editor for now.

		void Start()
		{
			rb2D = GetComponent<Rigidbody2D>();
			idol = Idol.Get().transform;
		}


		internal override void FixedMoveUpdate()
		{
			if( rb2D != null )
			{
				if( HasIdol )
					RunAway();
				else
					MoveToIdol();


				/*
				Vector2 targetDir = VectorExtras.Direction( VectorExtras.V2FromV3(this.transform.position), VectorExtras.V2FromV3(idol.position));
				rb2D.AddForce( targetDir * force, ForceMode2D.Force );

				TransformExtensions.SetRotation2D(this.transform, VectorExtras.VectorToDegrees(rb2D.velocity.normalized));
				*/
			}

		}

		void MoveToIdol()
		{
			MoveToPosition( idol );
		}
		void OnCollisionEnter2D( Collision2D col )
		{
			if( owner.isImmobile == false )
			{
				if( col.transform.tag == "Idol" )
				{
					Idol.Get().Pickup( this.owner );
					idol.position = holdingPosition.position;
					idol.parent = holdingPosition;
					if (idolGrabbedSounds != null && idolGrabbedSounds.Length > 0)
						AudioHelper.PlayClipAtPoint(idolGrabbedSounds[Random.Range(0, idolGrabbedSounds.Length)], transform.position);
				}
			}
		}
		
		void RunAway()
		{
			Vector3 awayFromPlayer = VectorExtras.Direction( Vector3.zero, this.transform.position ) * 25.0f;
			MoveToPosition( awayFromPlayer );
		}
		//This method will always run, regardless of if this component is disabled or not.
		internal override void AlwaysUpdate()
		{
			base.AlwaysUpdate();

			if( owner.isImmobile == true && HasIdol )
			{
				DropIdol();
			}
		}
		void OnDestroy()
		{
			if( HasIdol )
			{
				DropIdol();
			}
		}
		void DropIdol()
		{
			Idol.Get().Drop( this.owner );
			foreach( Transform child in holdingPosition )
				child.parent = null;
		}


		void MoveToPosition( Transform pos )
		{
			MoveToPosition( pos.position );
		}
		void MoveToPosition( Vector3 pos )
		{
			MoveToPosition( new Vector2(pos.x, pos.y) );
		}
		void MoveToPosition( Vector2 pos )
		{
			Vector2 targetDir = VectorExtras.Direction( VectorExtras.V2FromV3(this.transform.position), pos );
			rb2D.AddForce( targetDir * force, ForceMode2D.Force );
			
			TransformExtensions.SetRotation2D(this.transform, VectorExtras.VectorToDegrees(rb2D.velocity.normalized));
		}








	}
}