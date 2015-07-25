using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enemies.Modules;

//Stephan Ennen 7/24/15

//This acts as the AI core for all regular enemies. Includes things like getting situational info and general functions.

namespace Enemies //DON'T extend this class. No need to.
{
	public class Enemy : MonoBehaviour 
	{
		public static Transform container;

		#region Situational Values
		[SerializeField] private bool hooked; //
		public bool isHooked { 
			get { return hooked; } 
			set 
			{
				gameObject.GetComponent<Rigidbody2D>().isKinematic = value;
				gameObject.layer = !value ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("IgnorePhysics");

				if( HookComponent != null )
					HookComponent.NotifyStatus( value );

				hooked = value;
			}
		}

		[SerializeField] private bool slam; //
		public bool inSlam { 
			get { return slam; }
			internal set {
				if( GroundComponent != null )
					GroundComponent.Active = !value;
				slam = value;
			}
		}

		[SerializeField] private bool fallen; //TODO make modular
		public bool hasFallen { 
			get { return fallen; }
			internal set {
				//Disable all the things...
				fallen = value;
			}
		}

		[SerializeField] private bool immobile;
		public bool isImmobile {
			get { return immobile; }
			internal set {
				//Disable all the things...
				immobile = value;
			}
		}

		#endregion Situational Values

		#region Health Stuffs
		[SerializeField] private int my_maxHp = 10;
		public int MaxHealth{
			get{ return my_maxHp; }
			set
			{
				my_maxHp = Mathf.Max( 1, value );
			}
		}
		[SerializeField] private int my_hp;
		public int Health { 
			get{ return my_hp; }
			set
			{
				my_hp = Mathf.Min(value, my_maxHp);
				if( isLethal() )
					Kill();
			}
		}

		public bool isLethal( int incomingDamage )
		{ return my_hp - incomingDamage > 0 ? false : true; }
		private bool isLethal()
		{ return my_hp > 0 ? false : true; }
		public void Kill()
		{
			DeathComponent.OnKilled();
			isImmobile = true;
			GameObject.DestroyObject( this.gameObject );
		}
		#endregion


		//Enemy Component variables
		public ModuleBase[]   modules; //This is needed for custom update(s)
		public EnemyGrounded  GroundComponent { get; internal set; }
		public DeathBehaviour DeathComponent  { get; internal set; }
		public HookBehaviour  HookComponent   { get; internal set; }
		public SlamBehaviour  SlamComponent   { get; internal set; }

		void Awake()
		{
			if( container == null )
			{
				container = new GameObject("_Container-Enemies").GetComponent<Transform>();
				container.position = Vector3.zero;
			}
			this.transform.parent = container;
			Health = my_maxHp;
			#region INITALIZE COMPONENTS
			//TODO - Throw an error and quit if there's a required component missing... Also possibly make a loop so we get all modules
			modules = new ModuleBase[0];
			ModuleBase module;
			//Grounded
			module = GetComponent<EnemyGrounded>() as ModuleBase;
			if( module != null )
			{
				GroundComponent = module as EnemyGrounded;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
			}

			//Death
			module = GetComponent<DeathBehaviour>();
			if( module != null )
			{
				DeathComponent = module as DeathBehaviour;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
			}
			else
			{
				module = gameObject.AddComponent<DeathBehaviour>() as ModuleBase;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
				DeathComponent = module as DeathBehaviour;
			}

			//Hook
			module = GetComponent<HookBehaviour>();
			if( module != null )
			{
				HookComponent = module as HookBehaviour;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
			}
			else
			{
				module = gameObject.AddComponent<HookBehaviour>() as ModuleBase;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
				HookComponent = module as HookBehaviour;
			}

			//Slam
			module = GetComponent<SlamBehaviour>();
			if( module != null )
			{
				SlamComponent = module as SlamBehaviour;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
			}
			#endregion INITALIZE COMPONENTS
		}

		void Start()
		{
			StartCoroutine( "RestoreTimer" );
		}


		//private bool _lastShouldNotMove;
		internal bool shouldNotMove //Are we in a situation where our movement isnt allowed? Use isImmobile, not this.
		{ get { return isHooked || inSlam || hasFallen; } }

		void Update()
		{
			if( this.transform.parent == null ) //Always make sure we're in our container, that is.. if another script isn't controlling us.
				this.transform.parent = container;

			foreach( ModuleBase module in modules )
			{
				module.AlwaysUpdate();
			}
		}

		public float exitTime = 0f;
		private IEnumerator RestoreTimer() //TODO stop this timer if shouldNotMove changes to true ever.
		{
			if( shouldNotMove )
				exitTime = 2.5f;

			while( exitTime > 0.0f )
			{
				isImmobile = true;
				while( shouldNotMove )
				{
					exitTime = 2.5f;
					yield return null;
				}


				exitTime -= Time.deltaTime;
				yield return null;
			}

			isImmobile = false;
			yield return null;
			StartCoroutine( "RestoreTimer" );
		}











	}
}