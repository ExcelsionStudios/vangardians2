using UnityEngine;
using System.Collections;
using Enemies;

//Stephan Ennen 7/24/15

//A base class that forces all enemy behaviours to have a reference to the core Enemy class. Maybe some other stuff later.

namespace Enemies.Modules
{
	[RequireComponent(typeof( Enemy ))]
	public abstract class ModuleBase : MonoBehaviour 
	{
		internal Enemy owner;

		//Activates or deactivates this module. Component.enabled only restricts OnGUI, Update and other simmilar methods from running. 
		//(Modules may be running their own independent update methods.)
		private bool my_active;
		public bool Active {
			set {
				if( isActive == true && value == false )
				{
					//Do On-Disable type stuff
				}
				my_active = value;
				this.enabled = value;
			}
		}
		public bool isActive{ get{ return my_active; } }

		//This update will always be called, regardless of this module's enabled being set to false. (This wont be called if Enemy is disabled)
		internal virtual void AlwaysUpdate() //Called from Enemy.cs
		{

		}

		//NOTE: If you NEED to use Awake() or Update(), please see here for how to do so... http://answers.unity3d.com/questions/452601/c-inheriting-awake.html
		//      ALWAYS call 'base.METHOD();' before doing anything else!
		protected virtual void Awake()
		{
			owner = GetComponent< Enemy >();

			if( owner == null )
			{ Debug.LogError("AI Module does not have an Enemy component to read from! Disabling...", this); this.enabled = false; return; }
		}
		//Do nothing. Purely for consistency for inheiriting classes.
		protected virtual void Update()
		{
			return;
		}
		protected virtual void FixedUpdate()
		{
			return;
		}
	}
}