using UnityEngine;
using System.Collections;
using Enemies;
using Utils.Audio;

// Matt McGrath - 8/08/2015

// Pretty much just extends the base HookBehaviour and adds sounds. Will probably do more in the future.
namespace Enemies.Modules
{
	public class SimpleHookBehaviour : HookBehaviour 
	{
		public AudioClip[] objectHookedSounds;	// Set in Editor for now.
		
		// When the hook head first makes contact with us.
		public override void OnHookPushStart()
		{
			// Play the only Hooked Sound there is for now. TODO: Pick randomly from sound array, base it on the Enemy being hooked, etc.
			if (objectHookedSounds != null && objectHookedSounds.Length > 0)
				AudioHelper.PlayClipAtPoint(objectHookedSounds[Random.Range(0, objectHookedSounds.Length)], transform.position);
		}

		// Every frame that the inital hook fire pushes us, this is called.
		public override void OnHookPushing()
		{
			base.OnHookPushing();
		}

		// When the hookhead passes control over to the regular hooked-enemy logic, this is called.
		public override void OnHookPushEnd()
		{
			base.OnHookPushEnd();
		}

		// The hook just attached to us.
		public override void OnHookStart()
		{
			base.OnHookStart();
		}

		// Called every update while we're attached to the hook.
		public override void OnHookUpdate()
		{
			base.OnHookUpdate();
		}

		// The hook just detatched from us.
		public override void OnHookEnd()
		{
			base.OnHookEnd();
		}
	}
}