using System.Collections;
using UnityEngine;
using Enemies;

//Stephan Ennen 6/24/15

//A base class which will create a simple way of creating custom effects for when an enemy is attached to a hook.
namespace Enemies.Modules
{
	public class HookBehaviour : ModuleBase
	{
		
		//When the hook head first makes contact with us.
		internal virtual void OnHookPushStart()
		{
			Debug.Log("Hook first contact!");
		}
		
		//Every frame that the inital hook fire pushes us, this is called.
		internal virtual void OnHookPushing()
		{
			return;
		}
		//When the hookhead passes control over to the regular hooked-enemy logic, this is called.
		internal virtual void OnHookPushEnd()
		{
			Debug.Log("Hook finished making contact! OnHookStart will be called next.");
		}


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


		private Situation lastStatus;
		//Called from the player's hook script. DONT override. Also note that this isnt called every frame.
		public void NotifyStatus( Situation status ) //TODO make this include the inital hook contact state.
		{
			if( lastStatus != status )
			{
				if( lastStatus == Situation.InControl && status == Situation.BeingPushedByHook )
					OnHookPushStart();
				else if( lastStatus == Situation.BeingPushedByHook && status == Situation.Hooked ) 
				{
					OnHookPushEnd();
					OnHookStart();
				}
				else if( lastStatus == Situation.Hooked ) //The new status is something we don't care about.
					OnHookEnd();
			}
			lastStatus = status;
		}


		protected override void Update()
		{
			if( owner.Status == Situation.Hooked )
				OnHookUpdate();
			else if( owner.Status == Situation.BeingPushedByHook )
				OnHookPushing();
		}




	}
}