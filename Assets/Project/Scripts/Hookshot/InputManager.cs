using UnityEngine;
using System.Collections;

// Stephan -- Around 6/07/2015

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
