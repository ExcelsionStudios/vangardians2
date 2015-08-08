using UnityEngine;
using System.Collections;
using Utils.Audio;

// Matt McGrath - 8/08/2015

// Class that will handle the initialization of our various systems, store gameplay states, etc.
public class GameplayManager : MonoBehaviour 
{
	// Use this for initialization
	void Start() 
	{
		AudioHelper.EffectVolume = 1f;
		AudioHelper.MasterVolume = 1f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			AudioHelper.MasterVolume -= 0.1f;
			Debug.Log("Master Volume Down: " + AudioHelper.MasterVolume);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			AudioHelper.MasterVolume += 0.1f;
			Debug.Log("Master Volume Up: " + AudioHelper.MasterVolume);
		}
	}
}
