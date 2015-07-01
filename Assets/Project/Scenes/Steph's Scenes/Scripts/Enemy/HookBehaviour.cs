using System.Collections;
using UnityEngine;

//Stephan Ennen 6/24/15

//A base class which will create a simple way of creating custom effects for when an enemy is attached to a hook.
public abstract class HookBehaviour : MonoBehaviour 
{
	///////
	/// DON'T USE THIS CLASS YET. NOT FINISHED + DOESNT WORK.
	///////


	//Called as soon as our player hook makes contact.
	public virtual void OnHookConnect() //TODO Set up hook variables so we can access velocity info and other stuff.
	{
		return;
	}

	//Called every Update() that we are hooked.
	public virtual void OnHookUpdate()
	{
		return;
	}

	//Called after the hook is disconnected from us.
	public virtual void OnHookRelease()
	{
		return;
	}



	//////// 
	//Really you shouldnt need to change anything below this point... (And you shouldn't need to call any of this yourself)
	public void HookConnect()
	{
		
	}
	public void HookDisconnect()
	{

	}
}
