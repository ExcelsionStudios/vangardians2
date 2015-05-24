using UnityEngine;
using System.Collections;

// Written by Esai Solorio
// May 23, 2015

public class RetryScript : MonoBehaviour {

	public void restartScene(){
		Application.LoadLevel (Application.loadedLevel);
	}
}
