using UnityEngine;
using System.Collections;


public class InputManager : MonoBehaviour 
{
	public static InputManager instance;



	void Awake () 
	{
		if( instance == null )
			instance = this;
		else
			Destroy( this );
	}
	

	void Update () 
	{
		
	}
}
