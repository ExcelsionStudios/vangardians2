using UnityEngine;
using System.Collections;

public class ShowProjectileSelectorOnClick : MonoBehaviour {

	public Animator projectileSelectorAnimator;
	
	// Update is called once per frame

	void Update(){
		if(Input.GetMouseButtonDown(1)){
			//if(projectileSelectorAnimator.GetBool("transition_in")){
				//projectileSelectorAnimator.SetBool("transition_out", false);
			projectileSelectorAnimator.SetBool("transition_in", true);
			projectileSelectorAnimator.SetBool("transition_out", false);
			


		}
	}

}
