using UnityEngine;
using System.Collections;

public class SimpleChain : MonoBehaviour {

	//Script by Esai Sollrio
	//June 15, 2015

	// Use this for initialization
	public LineRenderer myLine;
	public Transform chainHead;
	public Transform chainBase;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		myLine.SetPosition (0, chainBase.position);
		myLine.SetPosition (1, chainHead.position);

	}
}
