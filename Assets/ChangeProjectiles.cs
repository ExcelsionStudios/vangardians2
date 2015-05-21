using UnityEngine;
using System.Collections;

public class ChangeProjectiles : MonoBehaviour {

	public FlingProjectilePrediction playerTower;
	public void changePrefab(GameObject obj){
		playerTower.projectilePrefab = obj;
	}
}
