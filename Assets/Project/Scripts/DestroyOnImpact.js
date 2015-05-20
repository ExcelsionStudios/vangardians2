#pragma strict

function Start () {

}

function Update () {

}
function OnCollisionEnter(col : Collision){
	if(col.gameObject.tag == "Enemy"){
		Destroy(col.gameObject);
	}
}