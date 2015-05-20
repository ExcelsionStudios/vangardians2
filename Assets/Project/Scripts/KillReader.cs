using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Matt McGrath - 5/18/2015

// Displays the Kill Count for the new Prototype.
public class KillReader : MonoBehaviour 
{
	public Text killText;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateScore();
	}
	
	// Updates the UI Text elements all Structures [may] share.
	void UpdateScore()
	{
		killText.text = "Kills: " + EnemyKillCounter.Kills;
	}
}
