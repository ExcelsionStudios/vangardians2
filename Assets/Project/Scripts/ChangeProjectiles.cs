using UnityEngine;
using System.Collections;
// Written by Esai Solorio
// May 22, 2015
public class ChangeProjectiles : MonoBehaviour {

	public FlingProjectilePrediction playerTower;
	public void changePrefab(GameObject obj){
		playerTower.projectilePrefab = obj;
	}
}
