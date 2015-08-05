using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen 8/5/15

namespace Enemies.Modules
{
	public class SwingBehaviour : ModuleBase 
	{
		internal float lastSwingTime; //Time.time in which we were swung. Might be useful for timed reactions.

		//Called right before swing force is applied to our rigidbody.
		internal virtual void OnSwing( bool isClockwiseDir, Vector3 forceDir )
		{
			Debug.Log("Swung "+ (isClockwiseDir ? "clockwise!" : "anti-clockwise!") );
		}

		public void NotifySwing( bool isClockwiseDir, Vector3 forceDir )
		{
			lastSwingTime = Time.time;
			OnSwing( isClockwiseDir, forceDir );
		}
	}
}