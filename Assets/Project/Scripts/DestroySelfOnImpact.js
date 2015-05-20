#pragma strict

function Start () {

}

function Update () {

}
function OnCollisionEnter(col : Collision){
	Destroy(gameObject);
}