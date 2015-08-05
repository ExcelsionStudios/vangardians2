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

		void OnDrawGizmos() //Quick and dirty distance debugging.
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere( transform.position, 1.3f );
		}

		[Tooltip("This enemy's current situation.")]
		[SerializeField] private Situation situation;
		public Situation Status
		{
			get{ return situation; }
			set{
				Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

				switch( value )
				{
				case Situation.InControl:
					this.gameObject.layer = LayerMask.NameToLayer("Enemy");
					rb2D.isKinematic = false;
					distJoint.enabled = false;
					break;
				case Situation.BeingPushedByHook:
					this.gameObject.layer = LayerMask.NameToLayer("IgnorePhysics");
					rb2D.isKinematic = true;
					distJoint.enabled = false;
					break;
				case Situation.Hooked:
					this.gameObject.layer = LayerMask.NameToLayer("Enemy");
					rb2D.isKinematic = false;
					distJoint.enabled = true;
					distJoint.connectedAnchor = VectorExtras.V2FromV3( Vector2.zero );
					distJoint.distance = Vector2.Distance( Vector2.zero, VectorExtras.V2FromV3(transform.position) );
					break;
				case Situation.BeingSlammed:
					this.gameObject.layer = LayerMask.NameToLayer("IgnorePhysics");
					rb2D.isKinematic = true;
					distJoint.enabled = false;
					break;
				}

				if( HookComponent != null )
					HookComponent.NotifyStatus( value );
				situation = value;
			}
		}
		#region Situational Values
		[Tooltip("If true, all movement modules will not execute.")]
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
		[Tooltip("How much hp this enemy starts with and is allowed to have at any time.")]
		[SerializeField] private int my_maxHp = 10;
		public int MaxHealth{
			get{ return my_maxHp; }
			set
			{
				my_maxHp = Mathf.Max( 1, value );
			}
		}
		[Tooltip("The current health value of this enemy.")]
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
		[Tooltip("Array of modules this enemy is using. This is set up automatically.")]
		public ModuleBase[]   modules; //This is needed for custom update(s)
		public EnemyGrounded  GroundComponent { get; internal set; }
		public DeathBehaviour DeathComponent  { get; internal set; }
		public HookBehaviour  HookComponent   { get; internal set; }
		public SlamBehaviour  SlamComponent   { get; internal set; }
		public MoveBehaviour  MoveComponent   { get; internal set; }
		public SwingBehaviour SwingComponent  { get; internal set; }
		private DistanceJoint2D distJoint;
		void Awake()
		{
			if( container == null )
			{
				container = new GameObject("_Container-Enemies").GetComponent<Transform>();
				container.position = Vector3.zero;
			}
			this.transform.parent = container;

			distJoint = GetComponent<DistanceJoint2D>();
			situation = Situation.InControl;
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

			//Move
			module = GetComponent<MoveBehaviour>();
			if( module != null )
			{
				MoveComponent = module as MoveBehaviour;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
			}
			//Swing
			module = GetComponent<SwingBehaviour>();
			if( module != null )
			{
				SwingComponent = module as SwingBehaviour;
				modules = ArrayTools.Push<ModuleBase>(modules, module);
			}
			#endregion
		}

		void Start()
		{
			StartCoroutine( "RestoreTimer" );
		}
		[Tooltip("While hooked and moving, this is the rate at which we get closer to the player.")]
		public float rangeLoss;
		void FixedUpdate()
		{
			if( Status == Situation.Hooked )
			{
				distJoint.distance = Mathf.Max(1.0f, distJoint.distance - GetComponent<Rigidbody2D>().velocity.magnitude * rangeLoss );
			}
		}


		void Update()
		{
			if( this.transform.parent == null ) //Always make sure we're in our container, that is.. if another script isn't controlling us.
				this.transform.parent = container;

			foreach( ModuleBase module in modules )
			{
				module.AlwaysUpdate();
			}
		}

		internal bool shouldNotMove //Are we in a situation where our movement isnt allowed? Use isImmobile, not this.
		{ get { return Status == Situation.BeingPushedByHook || Status == Situation.Hooked || Status == Situation.BeingSlammed; } }
		[Tooltip("If greater than zero, this enemy is not allowed to move.")]
		public float stunTimer = 0f;
		private IEnumerator RestoreTimer() //TODO stop this timer if shouldNotMove changes to true ever.
		{
			if( shouldNotMove )
				stunTimer = 2.5f;

			while( stunTimer > 0.0f )
			{
				isImmobile = true;
				while( shouldNotMove )
				{
					stunTimer = 2.5f;
					yield return null;
				}


				stunTimer -= Time.deltaTime;
				yield return null;
			}

			isImmobile = false;
			yield return null;
			StartCoroutine( "RestoreTimer" );
		}








	}
}