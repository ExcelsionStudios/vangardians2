using UnityEngine;
using System.Collections;

//Stephan Ennen 6/24/15

//A base class which will create a simple way of creating custom effects for an enemy that is being shoved by our player's hook.
public abstract class ShoveBehaviour : MonoBehaviour
{
	///////
	/// DON'T USE THIS CLASS YET. NOT FINISHED + DOESNT WORK.
	///////


	//Called when we are colliding with another enemy, (Because of us being pushed or swung by the hook) Apply extra damage or extra force here.
	//Keep in mind that the rigidbody attached to this script is set to kinematic during this!
	public virtual void OnShove( Rigidbody2D other )
	{
		return;
	}

	//Internal collision stuffs. If you absolutely need to use this function, be sure to call "base.OnCollisionStay2D()" in your override!!
	public virtual void OnCollisionStay2D( Collision2D col )
	{
		if( col.transform.tag == "Enemy" )
		{
			this.OnShove( col.rigidbody );
		}
	}




}
