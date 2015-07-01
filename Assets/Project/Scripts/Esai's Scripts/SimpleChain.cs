using System.Collections;
using UnityEngine;

//Script by Esai Sollrio
//June 15, 2015

public class SimpleChain : MonoBehaviour 
{
	public LineRenderer myLine;
	public Transform chainHead;
	public Transform chainBase;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		myLine.SetPosition (0, chainBase.position);
		myLine.SetPosition (1, chainHead.position);
	}
}
