#pragma strict
var target : Transform;
function Start () {

}

function Update () {
	
	gameObject.transform.rotation.z = target.rotation.z;
	gameObject.transform.rotation.y = target.rotation.y;
	
}