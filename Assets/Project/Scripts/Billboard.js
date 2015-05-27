#pragma strict
var target : Transform;

function Start () 
{

}

function Update () 
{
	
//	gameObject.transform.rotation.z = target.rotation.z;
//	gameObject.transform.rotation.y = target.rotation.y;
	
	// Matt: The above wasn't working. TODO: Make it LookAt camera of our choice, in case we aren't using main.
	transform.LookAt(Camera.main.transform);
}