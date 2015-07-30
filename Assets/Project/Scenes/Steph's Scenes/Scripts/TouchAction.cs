using UnityEngine;
using System.Collections;

//Stephan Ennen 7/30/15

//Script that refines touch/mouse information before SimpleHook.cs accesses it.
public class TouchAction : MonoBehaviour 
{
	//value for snapping a direction to an enemy direction
	public Vector3 startTouch;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.gray;
	}

	void Start () 
	{
		if( startTouch == Vector3.zero )
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 mPos = VectorExtras.GetMouseWorldPos();
				startTouch = new Vector3(mPos.x, mPos.y, 0f);
			}
		}
	}

	void Update () 
	{
	
	}
}
