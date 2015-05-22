using UnityEngine;
using System.Collections;

public class DealDamage : MonoBehaviour {

	// Use this for initialization
	public float power;
	public float rate;

	void Start(){
	//	gameObject.GetComponent<Animation> () ["Attack"].speed = rate;
	}

	public void doDamage(){
		transform.parent.GetComponent<MoveTo> ().target.GetComponent<PlayerTower> ().Health -= power;
	}


}
