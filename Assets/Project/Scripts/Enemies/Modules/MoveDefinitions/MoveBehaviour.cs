using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen 7/24/15

//Baseclass for any behaviour that moves the enemy. This class is more for identifying what modules will cause issues.
namespace Enemies.Modules
{
	public abstract class MoveBehaviour : ModuleBase
	{
		protected override void Update()
		{
			base.Update ();
			if( owner.isImmobile == false )
				MoveUpdate();
		}
		internal virtual void MoveUpdate()
		{
			return;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate ();
			if( owner.isImmobile == false )
				FixedMoveUpdate();
		}
		internal virtual void FixedMoveUpdate()
		{
			return;
		}
	}
}