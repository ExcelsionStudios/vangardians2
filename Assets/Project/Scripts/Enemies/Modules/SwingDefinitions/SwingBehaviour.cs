using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen 8/5/15

namespace Enemies.Modules
{
	public class SwingBehaviour : ModuleBase 
	{
		internal float lastSwingTime; //Time.time in which we were swung. Might be useful for timed reactions.

		// Matt: Lazily using the base class to add the swing sound, for now.
		public AudioClip[] swingSounds;		// Set in Editor for now.

		//Called right before swing force is applied to our rigidbody.
		internal virtual void OnSwing( bool isClockwiseDir, Vector3 forceDir )
		{
			Debug.Log("Swung "+ (isClockwiseDir ? "clockwise!" : "anti-clockwise!") );
			if (swingSounds != null && swingSounds.Length > 0)
				Utils.Audio.AudioHelper.PlayClipAtPoint(swingSounds[Random.Range(0, swingSounds.Length)], transform.position);
		}

		public void NotifySwing( bool isClockwiseDir, Vector3 forceDir )
		{
			lastSwingTime = Time.time;
			OnSwing( isClockwiseDir, forceDir );
		}
	}
}