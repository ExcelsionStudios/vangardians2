using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Matt McGrath - 5/19/2015

// Displays the score for the new Prototype. Score for now is simply number of kills.
public class WaveReader : MonoBehaviour 
{
	public Text waveNumberText;
	public Text timeRemainingText;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateWaveInformation();
	}
	
	// Updates the UI Text elements all Structures [may] share.
	void UpdateWaveInformation()
	{
		GameObject go = GameObject.Find("Wave Manager");
		EnemyWaveManager wm = go.GetComponent<EnemyWaveManager>();
		//EnemyWave wave = wm.GetComponent<EnemyWave>();

		waveNumberText.text = "Wave #" + wm.CurrentWaveNumber;

//		if (wave == null )
//		{
//			Debug.Log ("Wave is null");
//		}
//		else 
		{
			timeRemainingText.text = "Time Left: " + (int)wm.currentPrefabedWave.waveTimeRemaining; //wave.waveTimeRemaining;
		}
	}
}
