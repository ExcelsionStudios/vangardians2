using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Matt McGrath - 5/22/2015, with help from an article :(

// Script w e can attach to any object (in this case we just want ProjectileList so I'll name the file for that purpose) which will alter the time scale.
//  Essentially, we've heard from the team we should slow time down when the user is making their projectile decision. Seems cheap to me, but let's go ahead and add it!
public class ProjectileScrollList : MonoBehaviour 
{
	// The value at which we set the Time.timescale property. We want between 0f and 1f (we don't want to speed up time when player is making a decision).
	// TODO: Clamp value, not really necessary for now.
	public float timeScaleFactor;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Set time scale back to what, for now, the game will usually always be running-at. This will change later.
		Time.timeScale = 1.0f;

		PerformRaycast();
		Debug.Log ("Time Scale: " + Time.timeScale.ToString ());
	}

	// This function performs a raycast to detect if this object is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed
	void PerformRaycast ()               
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);                            // This section prepares a list for all objects hit with the raycast
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int count = objectsHit.Count;
		int x = 0;
		

		foreach (RaycastResult result in objectsHit)
		{
			if (result.gameObject == this.gameObject)                                         
			{    
				Debug.Log ("Scroll Wheel being hit");
				// This section runs only if this object is the front object where the cursor is
				Time.timeScale = 0.1f;
			}
		}
//		if(objectsHit[x].gameObject == this.gameObject)                                         
//		{    
//			Debug.Log ("Scroll Wheel being hit");
//			// This section runs only if this object is the front object where the cursor is
//			Time.timeScale = 0.1f;
//		}
//		else
//		{
//			Time.timeScale = 1.0f;
//		}
	}
}
