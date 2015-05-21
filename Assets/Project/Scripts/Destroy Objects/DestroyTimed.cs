using UnityEngine;
using System.Collections;

// Matt McGrath 5/20/2015

// Script which will destroy the object it's attached to after a given amount of time.
public class DestroyTimed : MonoBehaviour 
{
	public float TimeUntilDestroyed;		// Time (in seconds) until the object should destroy itself.

	void Start()
	{
		Destroy(gameObject, TimeUntilDestroyed);
	}
}
