using System.Collections;
using UnityEngine;
using Enemies;

//Stephan Ennen 6/24/15

//A base class which will create a simple way of creating custom effects for when an enemy is attached to a hook.
namespace Enemies.Modules
{
	public class HookBehaviour : ModuleBase
	{


		//The hook just attached to us.
		internal virtual void OnHookStart()
		{
			Debug.Log("Hook Attached!");
		}
		//Called every update while we're attached to the hook.
		internal virtual void OnHookUpdate()
		{
			return;
		}
		//The hook just detatched from us.
		internal virtual void OnHookEnd()
		{
			Debug.Log("Hook Dettached!");
		}

		//Called from the player's hook script. DONT override.
		public void NotifyStatus( bool state )
		{
			//gameObject.GetComponent<Rigidbody2D>().isKinematic = state;
			//gameObject.layer = state ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("IgnorePhysics");

			if( state == true && owner.isHooked != true )
			{
				OnHookStart();
			}
			else if( state == false && owner.isHooked != false )
			{
				OnHookEnd();
			}
			//owner.isHooked = state;
		}


		protected override void Update()
		{
			if( owner.isHooked == true )
			{
				OnHookUpdate();
			}
		}




	}
}