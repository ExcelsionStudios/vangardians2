using UnityEngine;
using System.Collections;

//Stephan Ennen 7/24/15

//Baseclass for doing things when an enemy dies (And when it is born)
namespace Enemies.Modules
{
	public class DeathBehaviour : ModuleBase
	{
		internal virtual void OnKilled()
		{
			Debug.Log("I'm ded :(", this);
		}
	}
}