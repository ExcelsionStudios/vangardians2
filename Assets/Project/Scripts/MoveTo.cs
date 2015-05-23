using UnityEngine;
using System.Collections;

// Matt McGrath 5/22/2015. I think MoveTo was a default script? So I hope me adding to it isn't stepping on anyone's shoes or overriding their credit.

// For prototype, simple way to get our Enemies (or any object) to move to a target at a certain speed. Now that we need to use a Time update to reduce their speed
// when the player is over the projectile selection wheel, this needs to be altered.
public class MoveTo : MonoBehaviour 
{
	public Transform target;
	public float speed;

	// Use this for initialization
	void Start () 
	{
	
	}


	// Update is called once per frame
	void Update () 
	{
		if (target != null) 
		{
			float step = speed * Time.deltaTime;
			gameObject.transform.LookAt(target);
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.position, step);
		}
	}

	// Esai Solorio Changes
	void LateUpdate(){
		Vector3 oldRotation = gameObject.transform.rotation.eulerAngles;  //save old rotation

		gameObject.transform.LookAt (target); //look at the target again
		Quaternion newRotation = new Quaternion ();
		newRotation.eulerAngles = new Vector3 (oldRotation.x, gameObject.transform.rotation.eulerAngles.y, oldRotation.z); //retain previous rotations except the Y axis
		gameObject.transform.rotation = newRotation;

	}
}
