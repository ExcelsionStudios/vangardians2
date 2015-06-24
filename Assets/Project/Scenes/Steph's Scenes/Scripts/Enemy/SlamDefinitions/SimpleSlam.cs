using UnityEngine;
using System.Collections;

//Stephan Ennen 6/24/15

//Simple behaviour that simply pushes all enemies away in a radius from the slam impact point.
public class SimpleSlam : SlamBehaviour 
{
	public float radius = 1f;
	public float speed = 10f;

	//Called as soon as the input identifies a slam. Maybe play sounds or a particle effect?
	public override void OnSlamStart()
	{
		return;
	}
	//Called every update while we are in the animation of the slam. (While we are flying through the air) Progress is a 0-1 value. (I thought it might be useful)
	public override void OnSlamUpdate( float progress )
	{
		return;
	}
	//Called at the very end of a slam. Do things like apply damage in an AOE or create particle effects, exc. (This is the primary use)
	public override void OnSlamEnd()
	{
		Vector2 center = VectorExtras.V2FromV3(transform.position);
		Debug.Log("Slamming effect "+ center, this);

		//Collider2D[] targets = Physics2D.OverlapCircleAll( center, radius, LayerMask.NameToLayer("Enemy") );
		Collider2D[] targets = Physics2D.OverlapCircleAll( center, radius, Physics2D.AllLayers );

		for( int i = 0; i < targets.Length; i++ )
		{
			Rigidbody2D body = targets[i].GetComponent<Rigidbody2D>();
			if( body == null )
				continue;

			Vector2 position = VectorExtras.V2FromV3(targets[i].transform.position);

			Vector2 awayFromCenter = VectorExtras.Direction( center, position );
			Vector2 awayFromPlayer = VectorExtras.Direction( Vector2.zero, position ); //TODO - VECTOR2.ZERO NEEDS TO BE REAL PLAYER POSITION!!
			Vector2 direction = Vector2.Lerp( awayFromCenter, awayFromPlayer, 0.65f );

			Debug.DrawRay( targets[i].transform.position, VectorExtras.V3FromV2(awayFromCenter, 0f), Color.red, 10f );
			Debug.DrawRay( targets[i].transform.position, VectorExtras.V3FromV2(awayFromPlayer, 0f), Color.blue, 10f );
			Debug.DrawRay( targets[i].transform.position, VectorExtras.V3FromV2(direction, 0f), Color.magenta, 10f );

			body.AddForceAtPosition( direction * speed, center, ForceMode2D.Impulse );
		}

	}
}
