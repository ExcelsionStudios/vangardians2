using UnityEngine;
using System.Collections;
// Written by Esai Solorio
// May 22, 2015
public class ChangeProjectiles : MonoBehaviour {

	public FlingProjectilePrediction playerTower;
	public Animator selectorUI;
	public UnityEngine.UI.Image currentPrefab;
	public void changePrefab(GameObject obj){
		playerTower.projectilePrefab = obj;
		selectorUI.SetBool ("transition_out", true);
		selectorUI.SetBool ("transition_in", false);


	}
}
