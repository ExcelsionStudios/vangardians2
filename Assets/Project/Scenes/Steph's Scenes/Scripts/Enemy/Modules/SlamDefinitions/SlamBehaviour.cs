using System.Collections;
using UnityEngine;
using Enemies;

//Stephan Ennen 6/24/15

// A base class which will create a simple way of creating custom effects for an enemy (or object, I guess) that is being slammed.
// BE SURE TO CALL THE BASE FUNCTION BEFORE ANYTHING ELSE.
namespace Enemies.Modules
{
	public abstract class SlamBehaviour : ModuleBase
	{
		// Called as soon as the input identifies a slam. Maybe play sounds or a particle effect?
		public virtual void OnSlamStart()
		{
			owner.Status = Situation.BeingSlammed;
			if( owner.GroundComponent != null )
				owner.GroundComponent.Active = false;
		}

		// Called every update while we are in the animation of the slam. (While we are flying through the air) Progress is a 0-1 value. (I thought it might be useful)
		public virtual void OnSlamUpdate( float progress )
		{
			return;
		}

		// Called at the very end of a slam. Do things like apply damage in an AOE or create particle effects, exc. (This is the primary use)
		public virtual void OnSlamEnd()
		{
			owner.Status = Situation.InControl;

			if( owner.GroundComponent != null )
				owner.GroundComponent.Active = true;
		}
	}
}